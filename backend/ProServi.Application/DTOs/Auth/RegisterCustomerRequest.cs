using System.ComponentModel.DataAnnotations;

namespace ProServi.Application.DTOs.Auth;

public class RegisterCustomerRequest
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "El nombre completo es requerido")]
    [StringLength(100, MinimumLength = 3)]
    public string FullName { get; set; }

    [Required(ErrorMessage = "El teléfono es requerido")]
    [Phone(ErrorMessage = "El teléfono no es válido")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    public string Password { get; set; }

    [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "La ciudad es requerida")]
    public string City { get; set; }

    [Required(ErrorMessage = "El país es requerido")]
    public string Country { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
