namespace Proyecto_Final_CapyGames_Back.Models
{
    public class DetallePedido
    {
        public int Id { set; get; }
        public int PedidoId { set; get; }
        public int ProductoId { set; get; }
        public int Cantidad { set; get; }
        public float PrecioUnitario { set; get; }
        public float Subtotal { set; get; }
    }
}
