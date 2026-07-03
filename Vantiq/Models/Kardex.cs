using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>
    /// Registro inmutable de cada movimiento de inventario. Fuente de verdad
    /// del stock: RELOJ.StockActual se recalcula a partir de estos asientos.
    /// </summary>
    [Table("KARDEX")]
    public class Kardex
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdKardex { get; set; }

        public int IdUsuario { get; set; }                      // quien registra el movimiento
        public short IdConcepto { get; set; }
        public int IdReloj { get; set; }
        public int? IdPedido { get; set; }                      // NULO: solo movimientos por venta

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int Cantidad { get; set; }

        public int StockResultante { get; set; }

        public DateTime FechaHoraMovimiento { get; set; }       // INMUTABLE (default GETDATE())

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; } = null!;

        [ForeignKey(nameof(IdConcepto))]
        public ConceptoKardex Concepto { get; set; } = null!;

        [ForeignKey(nameof(IdReloj))]
        public Reloj Reloj { get; set; } = null!;

        [ForeignKey(nameof(IdPedido))]
        public Pedido? Pedido { get; set; }
    }
}
