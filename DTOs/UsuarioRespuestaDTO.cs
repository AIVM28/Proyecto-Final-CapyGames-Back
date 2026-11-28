namespace Proyecto_Final_CapyGames_Back.DTOs;

public class UsuarioRespuestaDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = "";

    public string Apellidos { get; set; } = "";

    public string Email { get; set; } = "";

    public string Telefono { get; set; } = "";

    public string Rol { get; set; } = "";

    public DateTime FechaRegistro { get; set; }
}
