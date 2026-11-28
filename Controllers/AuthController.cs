using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Proyecto_Final_CapyGames_Back.DTOs;
using Proyecto_Final_CapyGames_Back.Services;

namespace Proyecto_Final_CapyGames_Back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("registro")]
    public IActionResult Registrar([FromBody] RegistroDto registro)
    {
        try
        {
            var usuario = _authService.Registrar(registro);

            return StatusCode(201, usuario);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                mensaje = ex.Message
            });
        }
    }

    [HttpPost("login")]
    public IActionResult IniciarSesion([FromBody] LoginDto login)
    {
        var respuesta = _authService.IniciarSesion(login);

        if (respuesta == null)
        {
            return Unauthorized(new
            {
                mensaje = "Correo o contraseña incorrectos."
            });
        }

        return Ok(respuesta);
    }


    [Authorize]
    [HttpGet("perfil")]
    public IActionResult ObtenerPerfil()
    {
        var idClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        if (!int.TryParse(idClaim, out int usuarioId))
        {
            return Unauthorized();
        }

        var perfil = _authService.ObtenerPerfil(usuarioId);

        if (perfil == null)
        {
            return NotFound(new
            {
                mensaje = "Usuario no encontrado."
            });
        }

        return Ok(perfil);
    }

    [Authorize]
    [HttpPut("perfil")]
    public IActionResult ActualizarPerfil(
        [FromBody] ActualizarPerfilDto perfil)
    {
        var idClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        if (!int.TryParse(idClaim, out int usuarioId))
        {
            return Unauthorized();
        }

        try
        {
            var actualizado = _authService.ActualizarPerfil(
                usuarioId,
                perfil
            );

            if (actualizado == null)
            {
                return NotFound(new
                {
                    mensaje = "Usuario no encontrado."
                });
            }

            return Ok(actualizado);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                mensaje = ex.Message
            });
        }
    }
}
