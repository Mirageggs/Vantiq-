using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>Categoria del reloj: Automatico, GMT, Cronografo.</summary>
    [Table("CATEGORIA")]
    public class Categoria
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ushort IdCategoria { get; set; }

        [Required, StringLength(50)]
        public string NombreCategoria { get; set; } = null!;    // UNICO

        [StringLength(150)]
        public string? Descripcion { get; set; }

        public bool EstaActiva { get; set; } = true;

        public ICollection<ModeloReloj> Modelos { get; set; } = new List<ModeloReloj>();
    }
}
