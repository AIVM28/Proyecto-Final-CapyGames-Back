namespace Proyecto_Final_CapyGames_Back.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        public string Apellidos { get; set; } = "";

        public string Email { get; set; } = "";

        public string PasswordHash { get; set; } = "";

        public string Telefono { get; set; } = "";

        public string Rol { get; set; } = "Cliente";

        public DateTime FechaRegistro { get; set; }

        public bool Activo { get; set; } = true;
    }
}
