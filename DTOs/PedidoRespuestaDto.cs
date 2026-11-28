
namespace Proyecto_Final_CapyGames_Back.DTOs;

public class PedidoRespuestaDto
{
    public int Id { get; set; }

    public int Folio { get; set; }

    public int DireccionId { get; set; }

    public DateTime Fecha { get; set; }

    public decimal Subtotal { get; set; }

    public decimal CostoEnvio { get; set; }

    public decimal Total { get; set; }

    public string MetodoPago { get; set; } = "";

    public string Estado { get; set; } = "";

    public List<DetallePedidoRespuestaDto> Detalles { get; set; } = new();
}
