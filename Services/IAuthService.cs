using Proyecto_Final_CapyGames_Back.DTOs;

namespace Proyecto_Final_CapyGames_Back.Services;

public interface IAuthService
{
    UsuarioRespuestaDto Registrar(RegistroDto registro);

    AuthRespuestaDto? IniciarSesion(LoginDto login);

    UsuarioRespuestaDto? ObtenerPerfil(int usuarioId);

    UsuarioRespuestaDto? ActualizarPerfil(
        int usuarioId,
        ActualizarPerfilDto perfil
    );
}
