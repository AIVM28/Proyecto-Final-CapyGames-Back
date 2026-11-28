namespace Proyecto_Final_CapyGames_Back.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public int Folio { get; set; }

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }

        public int DireccionId { get; set; }

        public Direccion? Direccion { get; set; }

        public DateTime Fecha { get; set; }

        public decimal Subtotal { get; set; }

        public decimal CostoEnvio { get; set; }

        public decimal Total { get; set; }

        public string MetodoPago { get; set; } = "";

        public string Estado { get; set; } = "Pendiente";

        public List<DetallePedido> DetallesPedido { get; set; }
            = new List<DetallePedido>();
    }
}
