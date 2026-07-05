using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vantiq.Data;
using Vantiq.Models;
using Vantiq.ViewModels;

namespace Vantiq.Controllers
{
    /// <summary>
    /// Panel de control del Administrador: KPIs y gráficos interactivos (Chart.js).
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
            var hoy       = DateTime.Today;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var hace30    = hoy.AddDays(-29);

            var ventasValidas = _db.Ventas.Where(v => v.EstadoVenta != EstadoVenta.Cancelado);

            var vm = new DashboardViewModel
            {
                VentasDelMes = await ventasValidas
                    .Where(v => v.FechaHoraVenta >= inicioMes)
                    .SumAsync(v => (decimal?)v.MontoTotal) ?? 0,

                VentasPendientes = await _db.Ventas
                    .CountAsync(v => v.EstadoVenta == EstadoVenta.Pendiente),

                ClientesRegistrados = await _db.UsuariosRoles
                    .CountAsync(ur => ur.IdRol == 2 && ur.Usuario.EstaActivo),

                StockCritico = await _db.Relojes
                    .Include(r => r.ModeloReloj)
                    .Include(r => r.Marca)
                    .Where(r => r.IdEstadoReloj != 3 && r.StockActual <= UmbralStockCritico)
                    .OrderBy(r => r.StockActual)
                    .ToListAsync()
            };

            // Serie: ventas por día (últimos 30 días, incluye días en cero)
            var porDia = await ventasValidas
                .Where(v => v.FechaHoraVenta >= hace30)
                .GroupBy(v => v.FechaHoraVenta.Date)
                .Select(g => new { Fecha = g.Key, Total = g.Sum(x => x.MontoTotal) })
                .ToListAsync();

            vm.VentasPorDia = Enumerable.Range(0, 30)
                .Select(i => hace30.AddDays(i))
                .Select(d => new PuntoSerie(
                    d.ToString("dd/MM"),
                    porDia.FirstOrDefault(p => p.Fecha == d)?.Total ?? 0))
                .ToList();

            // Top 5 relojes más vendidos (unidades, excluye ventas canceladas)
            vm.TopRelojes = (await _db.DetallesVenta
                    .Where(d => d.Venta.EstadoVenta != EstadoVenta.Cancelado)
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
