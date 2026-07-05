using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    [Table("MARCA")]
    public class Marca
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMarca { get; set; }

        [Required, StringLength(50)]
        public string NombreMarca { get; set; } = null!;    // ÚNICO

        public bool EstaActiva { get; set; } = true;

        public ICollection<Reloj> Relojes { get; set; } = new List<Reloj>();
    }
}
