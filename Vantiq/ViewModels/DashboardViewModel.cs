using Vantiq.Models;

namespace Vantiq.ViewModels
{
    public record PuntoSerie(string Etiqueta, decimal Valor);
    public record PuntoTop(string Etiqueta, int Valor);

    /// <summary>KPIs y series del panel del Administrador (Chart.js).</summary>
    public class DashboardViewModel
    {
        public decimal VentasDelMes { get; set; }
        public int VentasPendientes { get; set; }
        public int ClientesRegistrados { get; set; }
        public List<Reloj> StockCritico { get; set; } = new();
        public List<PuntoSerie> VentasPorDia { get; set; } = new();
        public List<PuntoTop> TopRelojes { get; set; } = new();
    }
}
