
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final_CapyGames_Back.DTOs;

public class ActualizarItemCarritoDto
{
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    public int Cantidad { get; set; }
}
