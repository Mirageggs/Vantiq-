using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>Categoría del reloj: Automático, GMT, Cronógrafo.</summary>
    [Table("CATEGORIA")]
    public class Categoria
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCategoria { get; set; }

        [Required, StringLength(50)]
        public string NombreCategoria { get; set; } = null!;    // ÚNICO

        [StringLength(150)]
        public string? Descripcion { get; set; }

        public bool EstaActiva { get; set; } = true;

        public ICollection<ModeloReloj> Modelos { get; set; } = new List<ModeloReloj>();
    }
}
