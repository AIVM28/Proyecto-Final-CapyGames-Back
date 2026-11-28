
namespace Proyecto_Final_CapyGames_Back.DTOs;

public class CarritoRespuestaDto
{
    public int Id { get; set; }

    public DateTime FechaActualizacion { get; set; }

    public List<DetalleCarritoRespuestaDto> Items { get; set; } = new();

    public decimal Subtotal { get; set; }

    public decimal CostoEnvio { get; set; }

    public decimal Total { get; set; }
}
