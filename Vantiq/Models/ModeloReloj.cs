using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>Modelo comercial del reloj (ej. Navigator), agrupado por categoría.</summary>
    [Table("MODELO_RELOJ")]
    public class ModeloReloj
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdModeloReloj { get; set; }

        [Required, StringLength(60)]
        public string NombreModelo { get; set; } = null!;   // ÚNICO

        public int IdCategoria { get; set; }

        public DateTime FechaHoraRegistro { get; set; }     // default GETDATE()

        public bool EstaActivo { get; set; } = true;

        [ForeignKey(nameof(IdCategoria))]
        public Categoria Categoria { get; set; } = null!;

        public ICollection<Reloj> Relojes { get; set; } = new List<Reloj>();
    }
}
