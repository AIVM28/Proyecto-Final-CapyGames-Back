using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final_CapyGames_Back.DTOs;

public class CrearPedidoDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una dirección válida.")]
    public int DireccionId { get; set; }

    [Required(ErrorMessage = "El método de pago es obligatorio")]
    public string MetodoPago { get; set; } = "";
}
