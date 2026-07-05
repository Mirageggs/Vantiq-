using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vantiq.Data;
using Vantiq.Models;

namespace Vantiq.Controllers
{
    /// <summary>
    /// Menú INVENTARIO del Administrador: catálogo de relojes y kardex.
    /// Regla de oro: el stock SOLO cambia mediante movimientos de kardex.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    public class InventarioController : Controller
    {
        private readonly VantiqDbContext _db;
        public InventarioController(VantiqDbContext db) => _db = db;

        private int IdAdmin => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> Index() =>
            View(await _db.Relojes
                .Include(r => r.ModeloReloj).ThenInclude(m => m.Categoria)
                .Include(r => r.Marca)
                .Include(r => r.EstadoReloj)
                .OrderBy(r => r.CodigoSKU)
                .ToListAsync());

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            await CargarCombos();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(int idModeloReloj, int idMarca, int numOrden,
            decimal precio, string urlImagen, string? descripcion, int stockInicial)
        {
            var modelo = await _db.ModelosReloj.FindAsync(idModeloReloj);
            var marca  = await _db.Marcas.FindAsync(idMarca);

            if (modelo == null || marca == null || numOrden < 1 || precio <= 0
                || string.IsNullOrWhiteSpace(urlImagen))
            {
                TempData["error"] = "Revisa los datos del formulario.";
                await CargarCombos();
                return View();
            }

            var sku = $"{marca.NombreMarca}-{modelo.NombreModelo.Replace("-", "")}-{numOrden:D3}".ToUpper();
            if (await _db.Relojes.AnyAsync(r => r.CodigoSKU == sku))
            {
                TempData["error"] = $"Ya existe un reloj con el SKU {sku}.";
                await CargarCombos();
                return View();
            }

            using var tx = await _db.Database.BeginTransactionAsync();
            var reloj = new Reloj
            {
                CodigoSKU      = sku,
                IdModeloReloj  = idModeloReloj,
                IdMarca        = idMarca,
                IdEstadoReloj  = stockInicial > 0 ? (byte)1 : (byte)2,
                Precio         = precio,
                UrlImagen      = urlImagen.Trim(),
                Descripcion    = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim(),
                NumOrden       = numOrden,
                StockActual    = 0
            };
            _db.Relojes.Add(reloj);
            await _db.SaveChangesAsync();

            if (stockInicial > 0)
            {
                reloj.StockActual = stockInicial;
                _db.MovimientosKardex.Add(new Kardex
                {
                    IdUsuario       = IdAdmin,
                    IdConcepto      = 1,    // Compra a proveedor
                    IdReloj         = reloj.IdReloj,
                    Cantidad        = stockInicial,
                    StockResultante = stockInicial
                });
                await _db.SaveChangesAsync();
            }
            await tx.CommitAsync();

            TempData["ok"] = $"Reloj {sku} registrado.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var reloj = await _db.Relojes
                .Include(r => r.ModeloReloj)
                .Include(r => r.Marca)
                .FirstOrDefaultAsync(r => r.IdReloj == id);
            if (reloj == null) return NotFound();
            ViewBag.Estados = await _db.EstadosReloj.Where(e => e.EstaActivo).ToListAsync();
            return View(reloj);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, decimal precio, string urlImagen,
            string? descripcion, byte idEstadoReloj)
        {
            var reloj = await _db.Relojes.FindAsync(id);
            if (reloj == null) return NotFound();
            if (precio <= 0 || string.IsNullOrWhiteSpace(urlImagen))
            {
                TempData["error"] = "Precio e imagen son obligatorios.";
                return RedirectToAction(nameof(Editar), new { id });
            }

            reloj.Precio       = precio;
            reloj.UrlImagen    = urlImagen.Trim();
            reloj.Descripcion  = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim();
            reloj.IdEstadoReloj = idEstadoReloj;
            await _db.SaveChangesAsync();

            TempData["ok"] = $"Reloj {reloj.CodigoSKU} actualizado.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Kardex(int id)
        {
            var reloj = await _db.Relojes
                .Include(r => r.ModeloReloj)
                .Include(r => r.Marca)
                .FirstOrDefaultAsync(r => r.IdReloj == id);
            if (reloj == null) return NotFound();

            ViewBag.Movimientos = await _db.MovimientosKardex
                .Include(k => k.Concepto)
                .Include(k => k.Usuario)
                .Where(k => k.IdReloj == id)
                .OrderByDescending(k => k.FechaHoraMovimiento)
                .ToListAsync();

            ViewBag.Conceptos = await _db.ConceptosKardex
                .Where(c => c.EstaActivo && (c.IdConcepto == 1 || c.IdConcepto == 4 || c.IdConcepto == 5))
                .ToListAsync();
            return View(reloj);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarMovimiento(int idReloj, byte idConcepto, int cantidad)
        {
            var reloj   = await _db.Relojes.FindAsync(idReloj);
            var concepto = await _db.ConceptosKardex.FindAsync(idConcepto);
            if (reloj == null || concepto == null) return NotFound();
            if (cantidad < 1)
            {
                TempData["error"] = "La cantidad debe ser mayor a cero.";
                return RedirectToAction(nameof(Kardex), new { id = idReloj });
            }
            if (concepto.TipoMovimiento == TipoMovimiento.SALIDA && reloj.StockActual < cantidad)
            {
                TempData["error"] = $"No puedes retirar {cantidad}: el stock actual es {reloj.StockActual}.";
                return RedirectToAction(nameof(Kardex), new { id = idReloj });
            }

            using var tx = await _db.Database.BeginTransactionAsync();

            if (concepto.TipoMovimiento == TipoMovimiento.ENTRADA)
                reloj.StockActual += cantidad;
            else
                reloj.StockActual -= cantidad;

            reloj.IdEstadoReloj = reloj.StockActual == 0 ? (byte)2 : (byte)1;

            _db.MovimientosKardex.Add(new Kardex
            {
                IdUsuario       = IdAdmin,
                IdConcepto      = idConcepto,
                IdReloj         = idReloj,
                Cantidad        = cantidad,
                StockResultante = reloj.StockActual
            });
            await _db.SaveChangesAsync();
            await tx.CommitAsync();

            TempData["ok"] = $"Movimiento registrado. Stock resultante: {reloj.StockActual}.";
            return RedirectToAction(nameof(Kardex), new { id = idReloj });
        }

        private async Task CargarCombos()
        {
            ViewBag.Modelos = await _db.ModelosReloj
                .Include(m => m.Categoria)
                .Where(m => m.EstaActivo)
                .OrderBy(m => m.NombreModelo)
                .ToListAsync();
            ViewBag.Marcas = await _db.Marcas.Where(m => m.EstaActiva).ToListAsync();
        }
    }
}
