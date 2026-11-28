namespace Proyecto_Final_CapyGames_Back.Models
{
    public class Producto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        public string Descripcion { get; set; } = "";

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public string ImagenUrl { get; set; } = "";

        public int CategoriaId { get; set; }

        public Categoria? Categoria { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaAlta { get; set; }
    }
}
