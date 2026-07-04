using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Vantiq.Models
{
    /// <summary>
    /// Pedido / venta del sistema. Su ciclo de vida sigue el diagrama de estados:
    /// Pendiente -> Pagado -> Enviado -> Entregado (o Cancelado).
    /// </summary>
    [Table("PEDIDO")]
    public class Pedido
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint IdPedido { get; set; }

        [Required, StringLength(20)]    
        public string CodigoPedido { get; set; } = null!;       // UNICO, ej. VQ-000125

        public uint IdCliente { get; set; }
        public ushort IdUsuario { get; set; }                     // NULO: admin que gestiona el pedido
        public byte IdMetodoPago { get; set; }
        public byte IdEstadoPedido { get; set; }

        [Precision(10, 2)]
        [Range(0.01, 99999999.99)]
        public decimal Total { get; set; }
                        
        public DateTime FechaHoraPedido { get; set; }           // INMUTABLE (default GETDATE())

        public bool EstaActivo { get; set; } = true;

        // Navegaciones
        [ForeignKey(nameof(IdCliente))]
        public Cliente Cliente { get; set; } = null!;

        [ForeignKey(nameof(IdUsuario))]
        public Usuario? Usuario { get; set; }

 

        [ForeignKey(nameof(IdEstadoPedido))]
        public EstadoPedido EstadoPedido { get; set; } = null!;

        public ICollection<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
        public ICollection<Kardex> MovimientosKardex { get; set; } = new List<Kardex>();
    }
}
