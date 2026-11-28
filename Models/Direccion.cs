namespace Proyecto_Final_CapyGames_Back.Models
{
    public class Direccion
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }

        public string Calle { get; set; } = "";

        public string NumeroExterior { get; set; } = "";

        public string? NumeroInterior { get; set; }

        public string Colonia { get; set; } = "";

        public string Ciudad { get; set; } = "";

        public string Estado { get; set; } = "";

        public string CodigoPostal { get; set; } = "";

        public string Referencias { get; set; } = "";

        public bool Predeterminada { get; set; } = false;
    }
}
