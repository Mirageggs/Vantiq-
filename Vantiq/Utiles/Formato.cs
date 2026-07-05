using Vantiq.Models;

namespace Vantiq.Utiles
{
    /// <summary>Ayudantes de presentación compartidos por las vistas.</summary>
    public static class Formato
    {
        public static string Soles(decimal monto) => "S/ " + monto.ToString("N2");

        public static string CodigoVenta(int idVenta) => "VQ-" + idVenta.ToString("D6");

        /// <summary>Clase CSS de badge según estado de la venta.</summary>
        public static string BadgeEstado(EstadoVenta estado) => estado switch
        {
            EstadoVenta.Pendiente  => "bg-warning text-dark",
            EstadoVenta.Pagado     => "bg-primary",
            EstadoVenta.Enviado    => "bg-info text-dark",
            EstadoVenta.Entregado  => "bg-success",
            _                     => "bg-danger"    // Cancelado
        };

        /// <summary>Clase CSS de badge según stock disponible.</summary>
        public static string BadgeStock(int stock) => stock switch
        {
            0    => "bg-danger",
            <= 3 => "bg-warning text-dark",
            _    => "bg-success"
        };
    }
}
