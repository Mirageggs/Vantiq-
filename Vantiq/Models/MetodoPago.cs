using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>Catálogo de métodos de pago aceptados (Yape, Plin, etc.).</summary>
    [Table("METODO_PAGO")]
    public class MetodoPago
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte IdMetodoPago { get; set; }

        [Required, StringLength(50)]
        public string NombreMetodoPago { get; set; } = null!;   // ÚNICO

        public bool EstaActivo { get; set; } = true;

        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
