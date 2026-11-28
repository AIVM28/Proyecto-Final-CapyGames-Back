
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Proyecto_Final_CapyGames_Back.DTOs;
using Proyecto_Final_CapyGames_Back.Services;

namespace Proyecto_Final_CapyGames_Back.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CarritoController : ControllerBase
{
    private readonly ICarritoService _carritoService;

    public CarritoController(ICarritoService carritoService)
    {
        _carritoService = carritoService;
    }

    // Consultar el carrito del usuario
    [HttpGet]
    public IActionResult ObtenerCarrito()
    {
        if (!ObtenerUsuarioId(out int usuarioId))
        {
            return Unauthorized();
        }

        var carrito = _carritoService.ObtenerCarrito(usuarioId);

        return Ok(carrito);
    }

    // Agregar un producto al carrito
    [HttpPost("items")]
    public IActionResult AgregarItem(
        [FromBody] AgregarItemCarritoDto item)
    {
        if (!ObtenerUsuarioId(out int usuarioId))
        {
            return Unauthorized();
        }

        try
        {
            var carrito = _carritoService.AgregarItem(
                usuarioId,
                item
            );

            return Ok(carrito);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // Actualizar cantidad de un producto
    [HttpPut("items/{id:int}")]
    public IActionResult ActualizarItem(
        int id,
        [FromBody] ActualizarItemCarritoDto item)
    {
        if (!ObtenerUsuarioId(out int usuarioId))
        {
            return Unauthorized();
        }

        try
        {
            var carrito = _carritoService.ActualizarItem(
                usuarioId,
                id,
                item
            );

            if (carrito == null)
            {
                return NotFound(new
                {
                    mensaje = "Artículo no encontrado en el carrito."
                });
            }

            return Ok(carrito);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // Eliminar un producto del carrito
    [HttpDelete("items/{id:int}")]
    public IActionResult EliminarItem(int id)
    {
        if (!ObtenerUsuarioId(out int usuarioId))
        {
            return Unauthorized();
        }

        bool eliminado = _carritoService.EliminarItem(usuarioId, id);

        if (!eliminado)
        {
            return NotFound(new
            {
                mensaje = "Artículo no encontrado en el carrito."
            });
        }

        return NoContent();
    }

    // Obtener el ID del usuario autenticado
    private bool ObtenerUsuarioId(out int usuarioId)
    {
        var idClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        return int.TryParse(idClaim, out usuarioId);
    }
}
