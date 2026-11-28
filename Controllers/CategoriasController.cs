using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Final_CapyGames_Back.Models;
using Proyecto_Final_CapyGames_Back.Services;

namespace Proyecto_Final_CapyGames_Back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    // GET: api/categorias
    [HttpGet]
    public IActionResult ObtenerTodas()
    {
        var categorias = _categoriaService.ObtenerTodas();

        return Ok(categorias);
    }

    // POST: api/categorias
    // SOLO ADMINISTRADOR
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public IActionResult Agregar([FromBody] Categoria categoria)
    {
        try
        {
            var nuevaCategoria = _categoriaService.Agregar(categoria);

            return StatusCode(201, nuevaCategoria);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // ACTUALIZAR CATEGORIA
    // PUT: api/categorias/{id}
    // SOLO ADMINISTRADOR
    [Authorize(Roles = "Administrador")]
    [HttpPut("{id:int}")]
    public IActionResult Actualizar(
        int id,
        [FromBody] Categoria categoria)
    {
        try
        {
            bool actualizado = _categoriaService.Actualizar(id, categoria);

            if (!actualizado)
            {
                return NotFound(new
                {
                    mensaje = "Categoría no encontrada."
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

    // DESACTIVAR CATEGORIA
    // DELETE: api/categorias/{id}
    // SOLO ADMINISTRADOR
    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id:int}")]
    public IActionResult Eliminar(int id)
    {
        bool desactivada = _categoriaService.Eliminar(id);

        if (!desactivada)
        {
            return NotFound(new
            {
                mensaje = "Categoría no encontrada."
            });
        }

        return NoContent();
    }
}
