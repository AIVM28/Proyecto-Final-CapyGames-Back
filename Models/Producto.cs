namespace Proyecto_Final_CapyGames_Back.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public float Precio { get; set; }
        public int Stock { get; set; } 
        public string ImagenUrl { get; set; } = "";
        public int CategoriaId { get; set; } 
        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}