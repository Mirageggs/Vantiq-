using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>Entidad asociativa N:M entre ROL y OPCION_MENU.</summary>
    [Table("ROL_OPCION_MENU")]
    public class RolOpcionMenu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ushort IdRolOpcionMenu { get; set; }

        public byte IdRol { get; set; }
        public byte IdOpcionMenu { get; set; }

        [ForeignKey(nameof(IdRol))]
        public Rol Rol { get; set; } = null!;

        [ForeignKey(nameof(IdOpcionMenu))]
        public OpcionMenu OpcionMenu { get; set; } = null!;
    }
}
