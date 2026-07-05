using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Vantiq.Models
{
    /// <summary>
    /// Orden de compra. Ciclo: Pendiente → Pagado → Enviado → Entregado (o Cancelado).
    /// IdClienteVisitante SIEMPRE se llena (snapshot de contacto/envío).
    /// IdUsuario es NULO cuando el comprador no tiene cuenta.
    /// </summary>
    [Table("VENTA")]
    public class Venta
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdVenta { get; set; }

        public int? IdUsuario { get; set; }         // NULO: comprador invitado sin cuenta
        public int IdClienteVisitante { get; set; }
        public byte IdMetodoPago { get; set; }

        [Precision(10, 2)]
        public decimal MontoTotal { get; set; }

        public EstadoVenta EstadoVenta { get; set; } = EstadoVenta.Pendiente;

        [Required, StringLength(250)]
        public string DireccionEnvio { get; set; } = null!;

        [StringLength(60)]
        public string? NumSeguimiento { get; set; }

        public DateTime FechaHoraVenta { get; set; }    // default GETDATE()

        // Navegaciones
        [ForeignKey(nameof(IdUsuario))]
        public Usuario? Usuario { get; set; }

        [ForeignKey(nameof(IdClienteVisitante))]
        public ClienteVisitante ClienteVisitante { get; set; } = null!;

        [ForeignKey(nameof(IdMetodoPago))]
        public MetodoPago MetodoPago { get; set; } = null!;

        public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
        public ICollection<Kardex> MovimientosKardex { get; set; } = new List<Kardex>();
    }
}
