using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>Estado del producto: 1 = Disponible, 2 = Agotado, 3 = Descontinuado.</summary>
    [Table("ESTADO_RELOJ")]
    public class EstadoReloj
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte IdEstadoReloj { get; set; }

        [Required, StringLength(30)]
        public string NombreEstadoReloj { get; set; } = null!;  // UNICO

        [StringLength(150)]
        public string? Descripcion { get; set; } = null!;

        public bool EstaActivo { get; set; } = true;

        public ICollection<Reloj> Relojes { get; set; } = new List<Reloj>();
    }
}
