
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final_CapyGames_Back.DTOs;

public class ActualizarEstadoPedidoDto
{
    [Required(ErrorMessage = "El estado es obligatorio")]
    public string Estado { get; set; } = "";
}
