using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vantiq.Data;
using Vantiq.Models;
using Vantiq.Services;
using Vantiq.ViewModels;

namespace Vantiq.Controllers
{
    /// <summary>
    /// Carrito 100 % en sesión (ICarritoService / ISession + JSON).
    /// El único acceso a BD ocurre en Checkout POST: crea ClienteVisitante,
    /// Venta, DetalleVenta y Kardex de salida en una transacción y luego
    /// vacía la sesión del carrito.
    /// </summary>
    public class CarritoController : Controller
    {
        private readonly VantiqDbContext _db;
        private readonly ICarritoService _carrito;

        public CarritoController(VantiqDbContext db, ICarritoService carrito)
        {
            _db = db;
            _carrito = carrito;
        }

        // GET /Carrito
        [HttpGet]
        public IActionResult Index()
        {
            var items = _carrito.Obtener();
            ViewBag.Total = _carrito.MontoTotal();
            return View(items);
        }

        // POST /Carrito/Agregar  (llamado desde Catalogo/Detalle)
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(int idReloj, short cantidad, string? returnUrl)
        {
            var reloj = await _db.Relojes
                .Include(r => r.ModeloReloj)
                .Include(r => r.Marca)
                .FirstOrDefaultAsync(r => r.IdReloj == idReloj && r.IdEstadoReloj != 3);

            if (reloj == null || reloj.StockActual == 0)
            {
                TempData["error"] = "Reloj no disponible.";
                return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                    ? Redirect(returnUrl)
                    : RedirectToAction("Index", "Catalogo");
            }

            if (cantidad <= 0) cantidad = 1;
            var enCarrito = _carrito.CantidadDe(idReloj);
            var disponible = (short)(reloj.StockActual - enCarrito);
            if (disponible <= 0)
            {
                TempData["warn"] = "No hay más unidades disponibles de este reloj.";
            }
            else
            {
                if (cantidad > disponible) cantidad = disponible;
                _carrito.Agregar(reloj, cantidad);
                TempData["ok"] = $"{reloj.CodigoSKU} añadido al carrito.";
            }

            return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                ? Redirect(returnUrl)
                : RedirectToAction("Index", "Catalogo");
        }

        // POST /Carrito/Actualizar
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Actualizar(int idReloj, short cantidad)
        {
            _carrito.ActualizarCantidad(idReloj, cantidad);
            return RedirectToAction(nameof(Index));
        }

        // POST /Carrito/Quitar
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Quitar(int idReloj)
        {
            _carrito.Quitar(idReloj);
            return RedirectToAction(nameof(Index));
        }

        // GET /Carrito/Checkout
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var items = _carrito.Obtener();
            if (items.Count == 0)
            {
                TempData["warn"] = "Tu carrito está vacío.";
                return RedirectToAction(nameof(Index));
            }

            var vm = new CheckoutViewModel();

            // Pre-cargar datos del usuario autenticado si los tiene
            if (User.Identity?.IsAuthenticated == true)
            {
                var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(idStr, out int idUsuario))
                {
                    var usuario = await _db.Usuarios.FindAsync(idUsuario);
                    if (usuario != null)
                    {
                        vm.Nombres = usuario.NombreUsuario;
                        vm.Email   = usuario.Email;
                    }
                }
            }

            ViewBag.Items       = items;
            ViewBag.Total       = _carrito.MontoTotal();
            ViewBag.MetodosPago = await _db.MetodosPago
                .Where(m => m.EstaActivo).OrderBy(m => m.IdMetodoPago).ToListAsync();
            return View(vm);
        }

        // POST /Carrito/Checkout  — ÚNICO punto donde el carrito toca la BD
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel vm)
        {
            var items = _carrito.Obtener();

            if (items.Count == 0)
            {
                TempData["warn"] = "Tu carrito está vacío.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Items       = items;
                ViewBag.Total       = _carrito.MontoTotal();
                ViewBag.MetodosPago = await _db.MetodosPago
                    .Where(m => m.EstaActivo).OrderBy(m => m.IdMetodoPago).ToListAsync();
                return View(vm);
            }

            // IdUsuario: nulo cuando el comprador no tiene cuenta
            int? idUsuario = null;
            if (User.Identity?.IsAuthenticated == true)
            {
                var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(idStr, out int parsed)) idUsuario = parsed;
            }
            int idRegistrador = idUsuario ?? 1; // admin como registrador para pedidos de invitados

            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                // 1. Snapshot del comprador (ClienteVisitante)
                var cliente = new ClienteVisitante
                {
                    NombresCliVisit  = vm.Nombres,
                    ApellidosCliVisit = vm.Apellidos,
                    Email            = vm.Email,
                    NumCelular       = vm.NumCelular
                };
                _db.ClientesVisitantes.Add(cliente);
                await _db.SaveChangesAsync();

                // 2. Cabecera de venta
                var venta = new Venta
                {
                    IdUsuario          = idUsuario,
                    IdClienteVisitante = cliente.IdClienteVisitante,
                    IdMetodoPago       = vm.IdMetodoPago,
                    MontoTotal         = _carrito.MontoTotal(),
                    DireccionEnvio     = vm.DireccionEnvio,
                    EstadoVenta        = EstadoVenta.Pendiente
                };
                _db.Ventas.Add(venta);
                await _db.SaveChangesAsync();

                // 3. Cargar relojes para ajustar stock
                var idRelojes = items.Select(i => i.IdReloj).ToList();
                var relojes   = await _db.Relojes
                    .Where(r => idRelojes.Contains(r.IdReloj)).ToListAsync();

                foreach (var item in items)
                {
                    var reloj = relojes.First(r => r.IdReloj == item.IdReloj);

                    // DetalleVenta (precio histórico al momento de vender)
                    _db.DetallesVenta.Add(new DetalleVenta
                    {
                        IdVenta              = venta.IdVenta,
                        IdReloj              = item.IdReloj,
                        Cantidad             = item.Cantidad,
                        PrecioUnitarioVenta  = item.Precio
                    });

                    // Descuento de stock
                    reloj.StockActual -= item.Cantidad;
                    if (reloj.StockActual == 0) reloj.IdEstadoReloj = 2; // Agotado

                    // Kardex de salida (concepto 2 = "Venta")
                    _db.MovimientosKardex.Add(new Kardex
                    {
                        IdUsuario       = idRegistrador,
                        IdConcepto      = 2,
                        IdReloj         = item.IdReloj,
                        IdVenta         = venta.IdVenta,
                        Cantidad        = item.Cantidad,
                        StockResultante = reloj.StockActual
                    });
                }

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                // 4. Limpiar la sesión del carrito
                _carrito.Vaciar();

                // 5. Cargar venta con detalles para la vista de confirmación
                var ventaConfirmada = await _db.Ventas
                    .Include(v => v.Detalles).ThenInclude(d => d.Reloj)
                    .FirstAsync(v => v.IdVenta == venta.IdVenta);

                ViewBag.Celular = await _db.Negocios
                    .Select(n => n.NumCelular).FirstOrDefaultAsync() ?? "51999999999";
                return View("Confirmacion", ventaConfirmada);
            }
            catch
            {
                await tx.RollbackAsync();
                TempData["error"] = "Error al procesar el pedido. Intenta nuevamente.";
                ViewBag.Items       = items;
                ViewBag.Total       = _carrito.MontoTotal();
                ViewBag.MetodosPago = await _db.MetodosPago
                    .Where(m => m.EstaActivo).OrderBy(m => m.IdMetodoPago).ToListAsync();
                return View(vm);
            }
        }
    }
}
