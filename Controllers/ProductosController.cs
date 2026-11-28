
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Proyecto_Final_CapyGames_Back.Models;
using Proyecto_Final_CapyGames_Back.Services;

namespace Proyecto_Final_CapyGames_Back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    // Consultar productos con filtros opcionales
    [HttpGet]
    public IActionResult ObtenerTodos(
        [FromQuery] int? categoriaId,
        [FromQuery] string? buscar)
    {
        var productos = _productoService.ObtenerTodos(
            categoriaId,
            buscar
        );

        return Ok(productos);
    }

    // Consultar producto por ID
    [HttpGet("{id:int}")]
    public IActionResult ObtenerPorId(int id)
    {
        var producto = _productoService.ObtenerPorId(id);

        if (producto == null)
        {
            return NotFound(new
            {
                mensaje = "Producto no encontrado."
            });
        }

        return Ok(producto);
    }

    // Crear producto
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public IActionResult Agregar([FromBody] Producto producto)
    {
        try
        {
            var nuevoProducto = _productoService.Agregar(producto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = nuevoProducto.Id },
                nuevoProducto
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // Actualizar producto
    [Authorize(Roles = "Administrador")]
    [HttpPut("{id:int}")]
    public IActionResult Actualizar(
        int id,
        [FromBody] Producto producto)
    {
        try
        {
            bool actualizado = _productoService.Actualizar(
                id,
                producto
            );

            if (!actualizado)
            {
                return NotFound(new
                {
                    mensaje = "Producto no encontrado."
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

    // Desactivar producto
    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id:int}")]
    public IActionResult Eliminar(int id)
    {
        bool desactivado = _productoService.Eliminar(id);

        if (!desactivado)
        {
            return NotFound(new
            {
                mensaje = "Producto no encontrado."
            });
        }

        return NoContent();
    }
}
