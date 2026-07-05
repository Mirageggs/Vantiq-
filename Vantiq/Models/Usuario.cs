using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>Cuenta de acceso al sistema (Clientes registrados y Administradores).</summary>
    [Table("USUARIO")]
    public class Usuario
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(50, MinimumLength = 4)]
        public string NombreUsuario { get; set; } = null!;  // ÚNICO

        [Required, StringLength(100)]
        [EmailAddress(ErrorMessage = "Correo electrónico no válido")]
        public string Email { get; set; } = null!;          // ÚNICO

        [Required, StringLength(256)]
        public string ContraseniaHash { get; set; } = null!;   // hash BCrypt, nunca texto plano

        public DateTime FechaHoraRegistro { get; set; }     // default GETDATE()
        public DateTime FechaHoraModificacion { get; set; } // default GETDATE()

        public bool EstaActivo { get; set; } = true;

        // Navegaciones
        public ICollection<UsuarioRol> Roles { get; set; } = new List<UsuarioRol>();
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
        public ICollection<Kardex> MovimientosKardex { get; set; } = new List<Kardex>();
    }
}
