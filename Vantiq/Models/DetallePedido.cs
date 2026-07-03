using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Vantiq.Models
{
    /// <summary>Linea del pedido; conserva el precio historico al momento de vender.</summary>
    [Table("DETALLE_PEDIDO")]
    public class DetallePedido
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdDetallePedido { get; set; }

        public int IdPedido { get; set; }
        public int IdReloj { get; set; }

        [Range(1, 999, ErrorMessage = "La cantidad debe ser al menos 1")]
        public short Cantidad { get; set; }

        [Precision(10, 2)]
        public decimal PrecioUnitario { get; set; }             // precio historico al vender

        [ForeignKey(nameof(IdPedido))]
        public Pedido Pedido { get; set; } = null!;

        [ForeignKey(nameof(IdReloj))]
        public Reloj Reloj { get; set; } = null!;
    }
}
