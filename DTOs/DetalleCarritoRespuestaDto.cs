
namespace Proyecto_Final_CapyGames_Back.DTOs;

public class DetalleCarritoRespuestaDto
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public string NombreProducto { get; set; } = "";

    public string ImagenUrl { get; set; } = "";

    public decimal PrecioUnitario { get; set; }

    public int Cantidad { get; set; }

    public decimal Subtotal { get; set; }

    public int StockDisponible { get; set; }

    public bool ProductoActivo { get; set; }
}
