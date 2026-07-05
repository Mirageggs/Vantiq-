using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>
    /// Snapshot de contacto del comprador en el momento de la venta.
    /// SIEMPRE se llena (regla de venta). IdUsuario es nulo cuando el comprador
    /// es un invitado sin cuenta registrada.
    /// </summary>
    [Table("CLIENTE_VISITANTE")]
    public class ClienteVisitante
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdClienteVisitante { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios"), StringLength(80)]
        public string NombresCliVisit { get; set; } = null!;

        [Required(ErrorMessage = "Los apellidos son obligatorios"), StringLength(80)]
        public string ApellidosCliVisit { get; set; } = null!;

        [Required, StringLength(100)]
        [EmailAddress(ErrorMessage = "Correo electrónico no válido")]
        public string Email { get; set; } = null!;

        [StringLength(15)]
        [Phone(ErrorMessage = "Número de celular no válido")]
        public string? NumCelular { get; set; }    // WhatsApp de contacto / coordinar envío

        public bool EstaActivo { get; set; } = true;

        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
