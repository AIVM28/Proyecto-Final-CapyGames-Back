using Microsoft.AspNetCore.Hosting;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public class CategoriaJsonRepository : JsonRepositoryBase<Categoria>, ICategoriaRepository
{
    public CategoriaJsonRepository(IWebHostEnvironment environment)
        : base(
            environment,
            "categorias.json",
            categoria => new Categoria
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Activa = categoria.Activa
            },
            categoria => categoria.Id,
            (categoria, id) => categoria.Id = id)
    {
    }

    public List<Categoria> ObtenerTodas() => ObtenerTodosBase();

    public Categoria? ObtenerPorId(int id) => ObtenerPorIdBase(id);

    public Categoria Agregar(Categoria nuevaCategoria) => AgregarBase(nuevaCategoria);

    public bool Actualizar(int id, Categoria categoria)
    {
        return ActualizarBase(id, existente =>
        {
            existente.Nombre = categoria.Nombre;
            existente.Descripcion = categoria.Descripcion;
            existente.Activa = categoria.Activa;
        });
    }

    public bool Eliminar(int id) => EliminarBase(id);
}
