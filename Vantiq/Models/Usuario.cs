using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>Cuenta de acceso al sistema (Clientes registrados y Administradores).</summary>
    [Table("USUARIO")]
    public class Usuario
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ushort IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(50, MinimumLength = 4)]
        public string NombreUsuario { get; set; } = null!;      // UNICO

        [Required, StringLength(100)]
        [EmailAddress(ErrorMessage = "Correo electronico no valido")]
        public string Email { get; set; } = null!;              // UNICO

        [Required, StringLength(256)]
        public string ContraseniaHash { get; set; } = null!;    // hash BCrypt, nunca texto plano

        public DateTime FechaHoraRegistro { get; set; }         // INMUTABLE (default GETDATE())
        public DateTime FechaHoraModificacion { get; set; }

        public bool EstaActivo { get; set; } = true;

        // Navegaciones
        public ICollection<UsuarioRol> Roles { get; set; } = new List<UsuarioRol>();
        public ICollection<Pedido> PedidosGestionados { get; set; } = new List<Pedido>();
        public ICollection<Kardex> MovimientosKardex { get; set; } = new List<Kardex>();
        public Cliente? Cliente { get; set; }                   // 1 : 0..1
    }
}
