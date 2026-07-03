using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>1 Yape, 2 Plin, 3 Transferencia, 4 Tarjeta, 5 Efectivo en tienda.</summary>
    [Table("METODO_PAGO")]
    public class MetodoPago
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte IdMetodoPago { get; set; }

        [Required, StringLength(40)]
        public string NombreMetodoPago { get; set; } = null!;   // UNICO

        public bool EstaActivo { get; set; } = true;

        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
