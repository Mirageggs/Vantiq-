using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vantiq.Data;
using Vantiq.Models;

namespace Vantiq.Controllers
{
    public class VentasController : Controller
    {
        private static readonly Dictionary<EstadoVenta, EstadoVenta[]> Transiciones = new()
        {
            [EstadoVenta.Pendiente] = new[] { EstadoVenta.Pagado,    EstadoVenta.Cancelado },
            [EstadoVenta.Pagado]    = new[] { EstadoVenta.Enviado,   EstadoVenta.Cancelado },
            [EstadoVenta.Enviado]   = new[] { EstadoVenta.Entregado }
        };

        private readonly VantiqDbContext _db;
        public VentasController(VantiqDbContext db) => _db = db;

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Index(byte? estado)
        {
            var consulta = _db.Ventas
                .Include(v => v.ClienteVisitante)
                .Include(v => v.Usuario)
                .AsQueryable();

            if (estado.HasValue)
                consulta = consulta.Where(v => v.EstadoVenta == (EstadoVenta)estado.Value);

            ViewBag.Estado = estado;
            ViewBag.Conteos = await _db.Ventas
                .GroupBy(v => (byte)v.EstadoVenta)
                .Select(g => new { Estado = g.Key, Total = g.Count() })
                .ToDictionaryAsync(x => x.Estado, x => x.Total);

            return View(await consulta.OrderByDescending(v => v.FechaHoraVenta).ToListAsync());
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var venta = await _db.Ventas
                .Include(v => v.ClienteVisitante)
                .Include(v => v.Usuario)
                .Include(v => v.MetodoPago)
                .Include(v => v.Detalles).ThenInclude(d => d.Reloj).ThenInclude(r => r.ModeloReloj)
                .FirstOrDefaultAsync(v => v.IdVenta == id);

            if (venta == null) return NotFound();

            ViewBag.Permitidas = Transiciones.TryGetValue(venta.EstadoVenta, out var siguientes)
                ? siguientes
                : Array.Empty<EstadoVenta>();

            return View(venta);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int idVenta, byte nuevoEstado)
        {
            var venta = await _db.Ventas
                .Include(v => v.Detalles).ThenInclude(d => d.Reloj)
                .FirstOrDefaultAsync(v => v.IdVenta == idVenta);

            if (venta == null) return NotFound();

            var nuevoEstadoEnum = (EstadoVenta)nuevoEstado;
            var valida = Transiciones.TryGetValue(venta.EstadoVenta, out var siguientes)
                         && siguientes.Contains(nuevoEstadoEnum);

            if (!valida)
            {
                TempData["error"] = "Transición no permitida.";
                return RedirectToAction(nameof(Detalle), new { id = idVenta });
            }

            if (nuevoEstadoEnum == EstadoVenta.Cancelado)
            {
                using var tx = await _db.Database.BeginTransactionAsync();
                var idAdmin = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                foreach (var det in venta.Detalles)
                {
                    det.Reloj.StockActual += det.Cantidad;
                    if (det.Reloj.IdEstadoReloj == 2) det.Reloj.IdEstadoReloj = 1;

                    _db.MovimientosKardex.Add(new Kardex
                    {
                        IdUsuario       = idAdmin,
                        IdConcepto      = 3,        // Devolución de cliente
                        IdReloj         = det.IdReloj,
                        IdVenta         = venta.IdVenta,
                        Cantidad        = det.Cantidad,
                        StockResultante = det.Reloj.StockActual
                    });
                }
                venta.EstadoVenta = EstadoVenta.Cancelado;
                await _db.SaveChangesAsync();
                await tx.CommitAsync();
            }
            else
            {
                venta.EstadoVenta = nuevoEstadoEnum;
                await _db.SaveChangesAsync();
            }

            TempData["ok"] = $"La venta pasó al estado {nuevoEstadoEnum}.";
            return RedirectToAction(nameof(Detalle), new { id = idVenta });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MisCompras()
        {
            var idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var ventas = await _db.Ventas
                .Include(v => v.MetodoPago)
                .Include(v => v.Detalles).ThenInclude(d => d.Reloj)
                .Where(v => v.IdUsuario == idUsuario)
                .OrderByDescending(v => v.FechaHoraVenta)
                .ToListAsync();

            return View(ventas);
        }
    }
}
