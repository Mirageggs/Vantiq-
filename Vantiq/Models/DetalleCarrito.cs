using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>Linea del carrito: un reloj y su cantidad (unica por carrito).</summary>
    [Table("DETALLE_CARRITO")]
    public class DetalleCarrito
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdDetalleCarrito { get; set; }

        public int IdCarrito { get; set; }
        public int IdReloj { get; set; }

        [Range(1, 999, ErrorMessage = "La cantidad debe ser al menos 1")]
        public short Cantidad { get; set; }

        [ForeignKey(nameof(IdCarrito))]
        public Carrito Carrito { get; set; } = null!;

        [ForeignKey(nameof(IdReloj))]
        public Reloj Reloj { get; set; } = null!;
    }
}
