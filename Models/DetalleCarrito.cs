namespace Proyecto_Final_CapyGames_Back.Models
{
    public class DetalleCarrito
    {
        public int Id { set; get; }
        public int CarritoId { set; get; }
        public int ProductoId { set; get; }
        public int Cantidad { set; get; }
    }
}