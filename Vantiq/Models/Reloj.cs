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
        public uint IdReloj { get; set; }

        [Required, StringLength(40)]
        public string CodigoSKU { get; set; } = null!;          // UNICO: marca + modelo + numOrden

        public uint IdModeloReloj { get; set; }
        public uint IdMarca { get; set; }
        public byte IdEstadoReloj { get; set; }

        public int NumOrden { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; } = null!;           // ficha tecnica (cristal, movimiento, etc.)

        [Required, StringLength(250)]
        public string UrlImagen { get; set; } = null!;

        [Precision(10, 2)]
        [Range(0.01, 99999999.99, ErrorMessage = "El precio debe ser mayor a cero")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public uint StockActual { get; set; }

     

        public DateTime FechaHoraRegistro { get; set; }         // INMUTABLE (default GETDATE())

        // Navegaciones
        [ForeignKey(nameof(IdModeloReloj))]
        public ModeloReloj ModeloReloj { get; set; } = null!;

        [ForeignKey(nameof(IdMarca))]
        public Marca Marca { get; set; } = null!;

        [ForeignKey(nameof(IdEstadoReloj))]
        public EstadoReloj EstadoReloj { get; set; } = null!;

        
        public ICollection<DetallePedido> DetallesPedido { get; set; } = new List<DetallePedido>();
        public ICollection<Kardex> MovimientosKardex { get; set; } = new List<Kardex>();
    }
}
