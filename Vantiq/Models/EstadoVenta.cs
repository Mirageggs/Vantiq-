namespace Vantiq.Models
{
    /// <summary>Ciclo de vida de una venta. Se persiste como byte en BD.</summary>
    public enum EstadoVenta : byte
    {
        Pendiente  = 1,
        Pagado     = 2,
        Enviado    = 3,
        Entregado  = 4,
        Cancelado  = 5
    }
}
