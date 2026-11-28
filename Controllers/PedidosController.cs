
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Proyecto_Final_CapyGames_Back.DTOs;
using Proyecto_Final_CapyGames_Back.Services;

namespace Proyecto_Final_CapyGames_Back.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    // Crear pedido mediante checkout
    [HttpPost]
    public IActionResult CrearPedido([FromBody] CrearPedidoDto pedido)
    {
        if (!ObtenerUsuarioId(out int usuarioId))
        {
            return Unauthorized();
        }

        try
        {
            var nuevoPedido = _pedidoService.CrearPedido(
                usuarioId,
                pedido
            );

            return CreatedAtAction(
                nameof(ObtenerDetalle),
                new { id = nuevoPedido.Id },
                nuevoPedido
            );
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

    // Consultar historial de pedidos
    [HttpGet]
    public IActionResult ObtenerTodos()
    {
        if (!ObtenerUsuarioId(out int usuarioId))
        {
            return Unauthorized();
        }

        var pedidos = _pedidoService.ObtenerPorUsuario(usuarioId);

        return Ok(pedidos);
    }

    // Consultar detalle de un pedido
    [HttpGet("{id:int}")]
    public IActionResult ObtenerDetalle(int id)
    {
        if (!ObtenerUsuarioId(out int usuarioId))
        {
            return Unauthorized();
        }

        var pedido = _pedidoService.ObtenerDetalle(id, usuarioId);

        if (pedido == null)
        {
            return NotFound(new
            {
                mensaje = "Pedido no encontrado."
            });
        }

        return Ok(pedido);
    }

    // Cancelar pedido pendiente
    [HttpPut("{id:int}/cancelar")]
    public IActionResult Cancelar(int id)
    {
        if (!ObtenerUsuarioId(out int usuarioId))
        {
            return Unauthorized();
        }

        try
        {
            bool cancelado = _pedidoService.Cancelar(id, usuarioId);

            if (!cancelado)
            {
                return NotFound(new
                {
                    mensaje = "Pedido no encontrado."
                });
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                mensaje = ex.Message
            });
        }
    }

    private bool ObtenerUsuarioId(out int usuarioId)
    {
        var idClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        return int.TryParse(idClaim, out usuarioId);
    }
}
