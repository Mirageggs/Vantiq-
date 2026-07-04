using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vantiq.Data; // Asegúrate de que el namespace de tu DbContext sea correcto
using Vantiq.Models;

namespace Vantiq.Controllers
{
    public class VentasController : Controller
    {
        // Replicamos los IDs de tu tabla ESTADO_PEDIDO para usarlos en la lógica
        public static class EstadosPedido
        {
            public const byte Pendiente = 1;
            public const byte Pagado = 2;
            public const byte Enviado = 3;
            public const byte Entregado = 4;
            public const byte Cancelado = 5;
        }

        private static readonly Dictionary<byte, byte[]> Transiciones = new()
        {
            [EstadosPedido.Pendiente] = new[] { EstadosPedido.Pagado, EstadosPedido.Cancelado },
            [EstadosPedido.Pagado] = new[] { EstadosPedido.Enviado, EstadosPedido.Cancelado },
            [EstadosPedido.Enviado] = new[] { EstadosPedido.Entregado }
        };

        private readonly VantiqDbContext _db;
        public VentasController(VantiqDbContext db) => _db = db;

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Index(byte? estado)
        {
            var consulta = _db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Usuario)
                .Include(p => p.EstadoPedido)
                .AsQueryable();

            if (estado.HasValue)
                consulta = consulta.Where(p => p.IdEstadoPedido == estado.Value);

            ViewBag.Estado = estado;
            ViewBag.Conteos = await _db.Pedidos
                .GroupBy(p => p.IdEstadoPedido)
                .Select(g => new { Estado = g.Key, Total = g.Count() })
                .ToDictionaryAsync(x => x.Estado, x => x.Total);

            return View(await consulta.OrderByDescending(p => p.FechaHoraPedido).ToListAsync());
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Detalle(uint id)
        {
            var pedido = await _db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Usuario)
                .Include(p => p.EstadoPedido)
                .Include(p => p.Detalles).ThenInclude(d => d.Reloj).ThenInclude(r => r.ModeloReloj)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null) return NotFound();

            ViewBag.Permitidas = Transiciones.TryGetValue(pedido.IdEstadoPedido, out var siguientes)
                ? siguientes
                : Array.Empty<byte>();

            return View(pedido);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(uint idPedido, byte nuevoEstado)
        {
            var pedido = await _db.Pedidos
                .Include(p => p.Detalles).ThenInclude(d => d.Reloj)
                .FirstOrDefaultAsync(p => p.IdPedido == idPedido);

            if (pedido == null) return NotFound();

            var valida = Transiciones.TryGetValue(pedido.IdEstadoPedido, out var siguientes)
                         && siguientes.Contains(nuevoEstado);
            if (!valida)
            {
                TempData["error"] = $"Transición no permitida.";
                return RedirectToAction(nameof(Detalle), new { id = idPedido });
            }

            if (nuevoEstado == EstadosPedido.Cancelado)
            {
                using var tx = await _db.Database.BeginTransactionAsync();

                // Parseo a ushort porque IdUsuario en Pedido es ushort
                var idAdmin = ushort.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                foreach (var det in pedido.Detalles)
                {
                    det.Reloj.StockActual += det.Cantidad;
                    if (det.Reloj.IdEstadoReloj == 2) det.Reloj.IdEstadoReloj = 1;

                    _db.MovimientosKardex.Add(new Kardex
                    {
                        IdUsuario = idAdmin,
                        IdConcepto = 3,
                        IdReloj = det.IdReloj,
                        IdPedido = pedido.IdPedido, // Actualizado de IdVenta a IdPedido
                        Cantidad = det.Cantidad,
                        StockResultante = det.Reloj.StockActual
                    });
                }
                pedido.IdEstadoPedido = EstadosPedido.Cancelado;
                await _db.SaveChangesAsync();
                await tx.CommitAsync();
            }
            else
            {
                pedido.IdEstadoPedido = nuevoEstado;
                await _db.SaveChangesAsync();
            }

            TempData["ok"] = $"El pedido pasó al estado {nuevoEstado}.";
            return RedirectToAction(nameof(Detalle), new { id = idPedido });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MisCompras()
        {
            // El ID extraído del token JWT pertenece al Cliente comprador
            var idCliente = uint.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var pedidos = await _db.Pedidos
                .Include(p => p.EstadoPedido)
                .Include(p => p.Detalles).ThenInclude(d => d.Reloj)
                .Where(p => p.IdCliente == idCliente)
                .OrderByDescending(p => p.FechaHoraPedido)
                .ToListAsync();

            return View(pedidos);
        }
    }
}