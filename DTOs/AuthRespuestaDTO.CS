namespace Proyecto_Final_CapyGames_Back.DTOs;

public class AuthRespuestaDto
{
    public string Token { get; set; } = "";

    public UsuarioRespuestaDto Usuario { get; set; } = new UsuarioRespuestaDto();
}
