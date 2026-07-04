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
        public ulong IdKardex { get; set; }

        public ushort IdUsuario { get; set; }                      // quien registra el movimiento
        public byte IdConcepto { get; set; }
        public uint IdReloj { get; set; }
        public uint? IdPedido { get; set; }                      // NULO: solo movimientos por venta

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public uint Cantidad { get; set; }

        public uint StockResultante { get; set; }

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
