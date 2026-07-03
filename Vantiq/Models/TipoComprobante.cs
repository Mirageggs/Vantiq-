using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>1 = Boleta, 2 = Factura.</summary>
    [Table("TIPO_COMPROBANTE")]
    public class TipoComprobante
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte IdTipoComprobante { get; set; }

        [Required, StringLength(20)]
        public string NombreTipo { get; set; } = null!;         // UNICO

        public bool EstaActivo { get; set; } = true;

        public ICollection<Comprobante> Comprobantes { get; set; } = new List<Comprobante>();
    }
}
