using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vantiq.Models
{
    /// <summary>
    /// Comprador (datos de contacto y comprobante). IdUsuario es NULO cuando
    /// el pedido lo realiza un Invitado sin cuenta.
    /// </summary>
    [Table("CLIENTE")]
    public class Cliente
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint IdCliente { get; set; }

        public ushort? IdUsuario { get; set; }                     // NULO: invitado sin cuenta

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [StringLength(80)]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(80)]
        public string Apellidos { get; set; } = null!;


        [Required, StringLength(100)]
        [EmailAddress(ErrorMessage = "Correo electronico no valido")]
        public string Email { get; set; } = null!;

        public bool EstaActivo { get; set; } = true;

        [ForeignKey(nameof(IdUsuario))]
        public Usuario? Usuario { get; set; }

        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
