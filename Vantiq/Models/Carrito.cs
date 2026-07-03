using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>
    /// Carrito persistente del Cliente autenticado (ventaja del rol Cliente:
    /// se conserva entre sesiones). El carrito del Invitado vive en Session.
    /// </summary>
    [Table("CARRITO")]
    public class Carrito
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCarrito { get; set; }

        public int IdUsuario { get; set; }

        public DateTime FechaHoraCreacion { get; set; }         // INMUTABLE (default GETDATE())
        public DateTime FechaHoraModificacion { get; set; }

        public bool EstaActivo { get; set; } = true;

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; } = null!;

        public ICollection<DetalleCarrito> Detalles { get; set; } = new List<DetalleCarrito>();
    }
}
