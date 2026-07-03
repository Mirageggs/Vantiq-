using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>Rol de acceso: 1 = Invitado, 2 = Cliente, 3 = Administrador.</summary>
    [Table("ROL")]
    public class Rol
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte IdRol { get; set; }

        [Required, StringLength(30)]
        public string NombreRol { get; set; } = null!;          // UNICO

        [StringLength(150)]
        public string? Descripcion { get; set; }

        public bool EstaActivo { get; set; } = true;

        public ICollection<UsuarioRol> UsuariosRol { get; set; } = new List<UsuarioRol>();
        public ICollection<RolOpcionMenu> OpcionesMenu { get; set; } = new List<RolOpcionMenu>();
    }
}
