
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Proyecto_Final_CapyGames_Back.DTOs;
using Proyecto_Final_CapyGames_Back.Services;

namespace Proyecto_Final_CapyGames_Back.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Administrador")]
public class AdminController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public AdminController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    // Consultar todos los pedidos y filtrar por estado
    [HttpGet("pedidos")]
    public IActionResult ObtenerPedidos([FromQuery] string? estado)
    {
        try
        {
            var pedidos = _pedidoService.ObtenerTodosAdmin(estado);

            return Ok(pedidos);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // Actualizar estado de un pedido
    [HttpPut("pedidos/{id:int}/estado")]
    public IActionResult ActualizarEstado(
        int id,
        [FromBody] ActualizarEstadoPedidoDto solicitud)
    {
        try
        {
            bool actualizado = _pedidoService.ActualizarEstadoAdmin(
                id,
                solicitud.Estado
            );

            if (!actualizado)
            {
                return NotFound(new
                {
                    mensaje = "Pedido no encontrado."
                });
            }

            return NoContent();
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

    // Consultar resumen administrativo
    [HttpGet("resumen")]
    public IActionResult ObtenerResumen()
    {
        var resumen = _pedidoService.ObtenerResumenAdmin();

        return Ok(resumen);
    }
}
