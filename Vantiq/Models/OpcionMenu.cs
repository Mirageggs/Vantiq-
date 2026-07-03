using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>Opcion de menu del sistema; se muestra segun el rol (RBAC).</summary>
    [Table("OPCION_MENU")]
    public class OpcionMenu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short IdOpcionMenu { get; set; }

        [Required, StringLength(60)]
        public string NombreOpcionMenu { get; set; } = null!;   // UNICO (indice en DbContext)

        [Required, StringLength(120)]
        public string UrlDestino { get; set; } = null!;

        public byte Orden { get; set; }          // posicion en el menu

        public bool EstaActiva { get; set; } = true;

        // Navegacion N:M con ROL (via entidad asociativa)
        public ICollection<RolOpcionMenu> RolesOpcionMenu { get; set; } = new List<RolOpcionMenu>();
    }
}
