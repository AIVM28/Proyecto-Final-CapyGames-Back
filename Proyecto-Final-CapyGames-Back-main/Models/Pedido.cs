namespace Proyecto_Final_CapyGames_Back.Models
{
    public class Pedido
    {
        public int Id { set; get; }
        public int Folio { set; get; }
        public int UsuarioId { set; get; }
        public int DireccionId {set; get; }
        public DateTime Fecha { set; get; }
        public float Subtotal { set; get; }
        public float CostoEnvio { set; get; }
        public float Total { set; get; }
        public string MetodoPago { set; get; } = "";
        public string Estado { set; get; } = "";
        public float ProductoCosto{ set; get; } 
    }
}