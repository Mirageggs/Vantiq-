using Vantiq.Models;

namespace Vantiq.Utiles
{
    /// <summary>Ayudantes de presentacion compartidos por las vistas.</summary>
    public static class Formato
    {
        public static string Soles(decimal monto) => "S/ " + monto.ToString("N2");

        // Se actualizó a 'uint' para que coincida con el IdPedido de tu base de datos
        public static string CodigoVenta(uint idPedido) => "VQ-" + idPedido.ToString("D6");

        // Evaluamos directamente usando los IDs numéricos (byte) de la tabla ESTADO_PEDIDO
        public static string BadgeEstado(byte idEstadoPedido) => idEstadoPedido switch
        {
            1 => "bg-warning text-dark", // Pendiente
            2 => "bg-primary",           // Pagado
            3 => "bg-info text-dark",    // Enviado
            4 => "bg-success",           // Entregado
            _ => "bg-danger"             // 5 = Cancelado (o cualquier otro)
        };

        // Sobrecarga opcional: por si en la vista pasas el objeto completo 'EstadoPedido'
        public static string BadgeEstado(EstadoPedido estado) => estado != null
            ? BadgeEstado(estado.IdEstadoPedido)
            : "bg-secondary";

        public static string BadgeStock(int stock) => stock switch
        {
            0 => "bg-danger",
            <= 3 => "bg-warning text-dark",
            _ => "bg-success"
        };
    }
}