namespace Proyecto_Final_CapyGames_Back.DTOs;

public class DetallePedidoRespuestaDto
{
    public int ProductoId { get; set; }

    public string NombreProducto { get; set; } = "";

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal { get; set; }
}