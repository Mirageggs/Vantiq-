using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Vantiq.Models
{
    /// <summary>Producto del catalogo. El stock se sincroniza con KARDEX.</summary>
    [Table("RELOJ")]
    public class Reloj
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdReloj { get; set; }

        [Required, StringLength(40)]
        public string CodigoSKU { get; set; } = null!;          // UNICO: marca + modelo + numOrden

        public int IdModeloReloj { get; set; }
        public short IdMarca { get; set; }
        public byte IdEstadoReloj { get; set; }

        public int NumOrden { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }                // ficha tecnica (cristal, movimiento, etc.)

        [Required, StringLength(250)]
        public string UrlImagen { get; set; } = null!;

        [Precision(10, 2)]
        [Range(0.01, 99999999.99, ErrorMessage = "El precio debe ser mayor a cero")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int StockActual { get; set; }

        [Range(0, 255)]
        public byte GarantiaMeses { get; set; }

        public DateTime FechaHoraRegistro { get; set; }         // INMUTABLE (default GETDATE())

        // Navegaciones
        [ForeignKey(nameof(IdModeloReloj))]
        public ModeloReloj ModeloReloj { get; set; } = null!;

        [ForeignKey(nameof(IdMarca))]
        public Marca Marca { get; set; } = null!;

        [ForeignKey(nameof(IdEstadoReloj))]
        public EstadoReloj EstadoReloj { get; set; } = null!;

        public ICollection<DetalleCarrito> DetallesCarrito { get; set; } = new List<DetalleCarrito>();
        public ICollection<DetallePedido> DetallesPedido { get; set; } = new List<DetallePedido>();
        public ICollection<Kardex> MovimientosKardex { get; set; } = new List<Kardex>();
    }
}
