using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>1 Pendiente, 2 Pagado, 3 Enviado, 4 Entregado, 5 Cancelado.</summary>
    [Table("ESTADO_PEDIDO")]
    public class EstadoPedido
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte IdEstadoPedido { get; set; }

        [Required, StringLength(30)]
        public string NombreEstadoPedido { get; set; } = null!; // UNICO

        [StringLength(150)]
        public string? Descripcion { get; set; } = null!;

        public bool EstaActivo { get; set; } = true;

        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
