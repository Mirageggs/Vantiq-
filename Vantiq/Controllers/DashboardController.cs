using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vantiq.Data;
using Vantiq.Models;
using Vantiq.ViewModels;

namespace Vantiq.Controllers
{
    /// <summary>
    /// Panel de control del Administrador: KPIs y graficos interactivos
    /// (Chart.js) en reemplazo de los reportes estaticos tradicionales.
    /// </summary>
    [Authorize(Roles = "Administrador")]
    public class DashboardController : Controller
    {
        private const int UmbralStockCritico = 3;
        private readonly VantiqDbContext _db;
        public DashboardController(VantiqDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var hoy = DateTime.Today;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var hace30 = hoy.AddDays(-29);

            // CORRECCIÓN: Tabla Pedidos y validación por IdEstadoPedido (5 = Cancelado)
            var ventasValidas = _db.Pedidos.Where(p => p.IdEstadoPedido != 5);

            var vm = new DashboardViewModel
            {
                // CORRECCIÓN: FechaHoraPedido y Total
                VentasDelMes = await ventasValidas
                    .Where(p => p.FechaHoraPedido >= inicioMes)
                    .SumAsync(p => (decimal?)p.Total) ?? 0,

                // CORRECCIÓN: Tabla Pedidos y validación (1 = Pendiente)
                VentasPendientes = await _db.Pedidos
                    .CountAsync(p => p.IdEstadoPedido == 1),

                ClientesRegistrados = await _db.UsuariosRoles
                    .CountAsync(ur => ur.IdRol == 2 && ur.Usuario.EstaActivo),

                StockCritico = await _db.Relojes
                    .Include(r => r.ModeloReloj)
                    .Include(r => r.Marca)
                    .Where(r => r.IdEstadoReloj != 3 && r.StockActual <= UmbralStockCritico)
                    .OrderBy(r => r.StockActual)
                    .ToListAsync()
            };

            // Serie: ventas por dia (ultimos 30 dias, incluye dias en cero)
            var porDia = await ventasValidas
                .Where(p => p.FechaHoraPedido >= hace30)
                .GroupBy(p => p.FechaHoraPedido.Date)
                .Select(g => new { Fecha = g.Key, Total = g.Sum(x => x.Total) })
                .ToListAsync();

            vm.VentasPorDia = Enumerable.Range(0, 30)
                .Select(i => hace30.AddDays(i))
                .Select(d => new PuntoSerie(
                    d.ToString("dd/MM"),
                    porDia.FirstOrDefault(p => p.Fecha == d)?.Total ?? 0))
                .ToList();

            // Top 5 relojes mas vendidos (unidades)
            // CORRECCIÓN: Tabla DetallesPedido e IdEstadoPedido != 5
            vm.TopRelojes = (await _db.DetallesPedido
                    .Where(d => d.Pedido.IdEstadoPedido != 5)
                    .GroupBy(d => d.Reloj.CodigoSKU)
                    .Select(g => new { Sku = g.Key, Unidades = g.Sum(x => (int)x.Cantidad) })
                    .OrderByDescending(x => x.Unidades)
                    .Take(5)
                    .ToListAsync())
                .Select(x => new PuntoTop(x.Sku, x.Unidades))
                .ToList();

            return View(vm);
        }
    }
}