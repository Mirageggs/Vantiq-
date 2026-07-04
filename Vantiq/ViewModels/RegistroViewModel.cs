using System.ComponentModel.DataAnnotations;

namespace Vantiq.ViewModels
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "Ingrese un nombre de usuario")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Nombre de usuario")]
        public string NombreUsuario { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese su correo")]
        [EmailAddress(ErrorMessage = "Correo no valido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese una contrasenia")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Minimo 8 caracteres")]
        [DataType(DataType.Password)]
        public string Contrasenia { get; set; } = null!;

        [Required(ErrorMessage = "Confirme la contrasenia")]
        [DataType(DataType.Password)]
        [Compare(nameof(Contrasenia), ErrorMessage = "Las contrasenias no coinciden")]
        [Display(Name = "Confirmar contrasenia")]
        public string ConfirmarContrasenia { get; set; } = null!;
    }
}
