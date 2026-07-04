namespace Vantiq.ViewModels
{
    /// <summary>Linea del carrito de sesion (se serializa como JSON).</summary>
    public class ItemCarrito
    {
        public int IdReloj { get; set; }
        public string CodigoSKU { get; set; } = null!;
        public string Nombre { get; set; } = null!;      // modelo + marca
        public string UrlImagen { get; set; } = null!;
        public decimal Precio { get; set; }
        public short Cantidad { get; set; }
        public decimal Subtotal => Precio * Cantidad;
    }
}
