using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>Boleta o factura emitida; relacion 1:0..1 con el pedido.</summary>
    [Table("COMPROBANTE")]
    public class Comprobante
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdComprobante { get; set; }

        public int IdPedido { get; set; }                       // UNICO: un comprobante por pedido
        public byte IdTipoComprobante { get; set; }

        [Required, StringLength(4)]
        public string Serie { get; set; } = null!;              // ej. B001 / F001

        public int Numero { get; set; }

        public DateTime FechaHoraEmision { get; set; }          // INMUTABLE (default GETDATE())

        public Pedido Pedido { get; set; } = null!;             // FK configurada por Fluent API (1:1)

        [ForeignKey(nameof(IdTipoComprobante))]
        public TipoComprobante TipoComprobante { get; set; } = null!;
    }
}
