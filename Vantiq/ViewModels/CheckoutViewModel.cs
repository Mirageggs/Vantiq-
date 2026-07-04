using System.ComponentModel.DataAnnotations;

namespace Vantiq.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Ingrese sus nombres"), StringLength(80)]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese sus apellidos"), StringLength(80)]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese su correo"), StringLength(100)]
        [EmailAddress(ErrorMessage = "Correo no valido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese su celular"), StringLength(15)]
        [Phone(ErrorMessage = "Celular no valido")]
        [Display(Name = "Celular (para coordinar pago y envio)")]
        public string NumCelular { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese la direccion de envio"), StringLength(200)]
        [Display(Name = "Direccion de envio (calle, numero, distrito, ciudad)")]
        public string DireccionEnvio { get; set; } = null!;

        [Range(1, 255, ErrorMessage = "Seleccione un metodo de pago")]
        [Display(Name = "Metodo de pago")]
        public byte IdMetodoPago { get; set; }
    }
}
