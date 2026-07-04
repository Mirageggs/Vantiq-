using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>Datos de configuracion del negocio (tabla de una sola fila).</summary>
    [Table("NEGOCIO")]
    public class Negocio
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte IdNegocio { get; set; }

        [Required(ErrorMessage = "El nombre del negocio es obligatorio")]
        [StringLength(100)]
        public string? NombreNegocio { get; set; } = null!;

        [StringLength(11, MinimumLength = 11, ErrorMessage = "El RUC debe tener 11 digitos")]
        public string? RucNegocio { get; set; }

        [StringLength(15)]
        [Phone(ErrorMessage = "Numero de celular no valido")]
        public string? NumCelular { get; set; } = null!;  // WhatsApp de contacto

        [StringLength(200)]
        public string? Direccion { get; set; }
    }
}
