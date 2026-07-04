using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>Entidad asociativa N:M entre USUARIO y ROL.</summary>
    [Table("USUARIO_ROL")]
    public class UsuarioRol
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint IdUsuarioRol { get; set; }

        public ushort IdUsuario { get; set; }
        public byte IdRol { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; } = null!;

        [ForeignKey(nameof(IdRol))]
        public Rol Rol { get; set; } = null!;
    }
}
