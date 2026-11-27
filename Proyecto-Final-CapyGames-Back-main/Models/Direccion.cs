namespace Proyecto_Final_CapyGames_Back.Models
{
    public class Direccion
    {
        public int Id { set; get; }
        public int UsuarioId { set; get; }
        public string Calle { set; get; } = "";
        public int NumeroExterior { set; get; }
        public int NumeroInterior { set; get; }
        public string Colonia { set; get; } = "";
        public string Ciudad { set; get; } = "";
        public string Estado { set; get; } = "";
        public int CodigoPostal { set; get; }
        public string Referencias { set; get; } = "";
        public string Predeterminada { set; get; } = "";
    }
}