
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final_CapyGames_Back.DTOs;

public class ActualizarPerfilDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = "";

    [Required(ErrorMessage = "Los apellidos son obligatorios")]
    public string Apellidos { get; set; } = "";

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [Phone(ErrorMessage = "El teléfono no tiene un formato válido")]
    public string Telefono { get; set; } = "";
}
