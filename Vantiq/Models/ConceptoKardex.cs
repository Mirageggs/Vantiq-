using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>Motivo del movimiento de inventario y su naturaleza (ENTRADA/SALIDA).</summary>
    [Table("CONCEPTO_KARDEX")]
    public class ConceptoKardex
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte IdConcepto { get; set; }

        [Required, StringLength(50)]
        public string NombreConcepto { get; set; } = null!;     // UNICO

        public TipoMovimiento TipoMovimiento { get; set; }      // enum persistido como nvarchar(10)

        [StringLength(150)]
        public string? Descripcion { get; set; } = null!;

        public bool EstaActivo { get; set; } = true;

        public ICollection<Kardex> Movimientos { get; set; } = new List<Kardex>();
    }
}
