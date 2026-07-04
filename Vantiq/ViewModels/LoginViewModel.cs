using System.ComponentModel.DataAnnotations;

namespace Vantiq.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ingrese su correo")]
        [EmailAddress(ErrorMessage = "Correo no valido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese su contrasenia")]
        [DataType(DataType.Password)]
        public string Contrasenia { get; set; } = null!;
    }
}
