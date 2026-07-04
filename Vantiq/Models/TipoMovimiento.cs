    namespace Vantiq.Models
{
    /// <summary>
    /// Tipo de movimiento de inventario. Reemplaza a la pseudo-tabla
    /// TIPO_MOVIMIENTO del borrador: se persiste como texto en CONCEPTO_KARDEX
    /// mediante conversion configurada en el DbContext.
    /// </summary>
    public enum TipoMovimiento
    {
        ENTRADA = 1,
        SALIDA = 2
    }
}
