namespace Proyecto_Final_CapyGames_Back.Models
{
    public class Carrito
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public List<DetalleCarrito> DetallesCarrito { get; set; } = new List<DetalleCarrito>();
    }
}
