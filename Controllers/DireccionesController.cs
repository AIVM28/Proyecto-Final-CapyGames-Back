using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Proyecto_Final_CapyGames_Back.Models;
using Proyecto_Final_CapyGames_Back.Services;

namespace Proyecto_Final_CapyGames_Back.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DireccionesController : ControllerBase
{
    private readonly IDireccionService _direccionService;

    public DireccionesController(IDireccionService direccionService)
    {
        _direccionService = direccionService;
    }

    // Consultar direcciones del usuario autenticado
    [HttpGet]
    public IActionResult ObtenerTodas()
    {
        if (!ObtenerUsuarioId(out int usuarioId))
            return Unauthorized();

        var direcciones = _direccionService.ObtenerPorUsuario(usuarioId);

        return Ok(direcciones);
    }

    // Agregar dirección
    [HttpPost]
    public IActionResult Agregar([FromBody] Direccion direccion)
    {
        if (!ObtenerUsuarioId(out int usuarioId))
            return Unauthorized();

        try
        {
            var nuevaDireccion = _direccionService.Agregar(
                usuarioId,
                direccion
            );

            return StatusCode(201, nuevaDireccion);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // Actualizar dirección
    [HttpPut("{id:int}")]
    public IActionResult Actualizar(
        int id,
        [FromBody] Direccion direccion)
    {
        if (!ObtenerUsuarioId(out int usuarioId))
            return Unauthorized();

        try
        {
            bool actualizado = _direccionService.Actualizar(
                id,
                usuarioId,
                direccion
            );

            if (!actualizado)
            {
                return NotFound(new
                {
                    mensaje = "Dirección no encontrada."
                });
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // Eliminar dirección
    [HttpDelete("{id:int}")]
    public IActionResult Eliminar(int id)
    {
        if (!ObtenerUsuarioId(out int usuarioId))
            return Unauthorized();

        try
        {
            bool eliminado = _direccionService.Eliminar(id, usuarioId);

            if (!eliminado)
            {
                return NotFound(new
                {
                    mensaje = "Dirección no encontrada."
                });
            }

            return NoContent();
        }
        catch (DbUpdateException)
        {
            return Conflict(new
            {
                mensaje = "No se puede eliminar la dirección porque está asociada a un pedido."
            });
        }
    }

    // Obtener ID del usuario desde el JWT
    private bool ObtenerUsuarioId(out int usuarioId)
    {
        var idClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        return int.TryParse(idClaim, out usuarioId);
    }
}
