using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Vantiq.Models
{
    /// <summary>Línea de venta; conserva el precio histórico al momento de vender.</summary>
    [Table("DETALLE_VENTA")]
    public class DetalleVenta
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdDetalleVenta { get; set; }

        public int IdVenta { get; set; }
        public int IdReloj { get; set; }

        [Range(1, short.MaxValue)]
        public short Cantidad { get; set; }

        [Precision(10, 2)]
        public decimal PrecioUnitarioVenta { get; set; }    // precio histórico al vender

        [ForeignKey(nameof(IdVenta))]
        public Venta Venta { get; set; } = null!;

        [ForeignKey(nameof(IdReloj))]
        public Reloj Reloj { get; set; } = null!;
    }
}
