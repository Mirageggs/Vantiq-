using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>
    /// Registro inmutable de cada movimiento de inventario.
    /// RELOJ.StockActual se mantiene sincronizado con estos asientos.
    /// </summary>
    [Table("KARDEX")]
    public class Kardex
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdKardex { get; set; }

        public int IdUsuario { get; set; }      // quien registra el movimiento
        public byte IdConcepto { get; set; }
        public int IdReloj { get; set; }
        public int? IdVenta { get; set; }       // NULO: movimientos manuales (ajustes)

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int Cantidad { get; set; }

        public int StockResultante { get; set; }

        public DateTime FechaHoraMovimiento { get; set; }   // default GETDATE()

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; } = null!;

        [ForeignKey(nameof(IdConcepto))]
        public ConceptoKardex Concepto { get; set; } = null!;

        [ForeignKey(nameof(IdReloj))]
        public Reloj Reloj { get; set; } = null!;

        [ForeignKey(nameof(IdVenta))]
        public Venta? Venta { get; set; }
    }
}
