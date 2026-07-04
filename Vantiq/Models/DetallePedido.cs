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
        public ulong IdDetallePedido { get; set; }

        public uint IdPedido { get; set; }
        public uint IdReloj { get; set; }

       
        [Precision(10, 2)]
        public decimal PrecioUnitario { get; set; }             // precio historico al vender

        [ForeignKey(nameof(IdPedido))]
        public Pedido? Pedido { get; set; } = null!;

        [ForeignKey(nameof(IdReloj))]
        public Reloj Reloj { get; set; } = null!;
        public uint Cantidad { get; internal set; }
    }
}
