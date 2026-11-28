using Proyecto_Final_CapyGames_Back.Models;
using Proyecto_Final_CapyGames_Back.Repositories;

namespace Proyecto_Final_CapyGames_Back.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repository;

    public CategoriaService(ICategoriaRepository repository)
    {
        _repository = repository;
    }

    public List<Categoria> ObtenerTodas()
    {
        return _repository.ObtenerTodas();
    }

    public Categoria? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public Categoria Agregar(Categoria nuevoCategoria)
    {
        ValidarCategoria(nuevoCategoria);

        return _repository.Agregar(nuevoCategoria);
    }

    public bool Actualizar(int id, Categoria Categoria)
    {
        var existente = _repository.ObtenerPorId(id);
        if (existente == null)
        {
            return false;
        }
        ValidarCategoria(Categoria);

        return _repository.Actualizar(id, Categoria);
    }

    public bool Eliminar(int id)
    {
        var existente = _repository.ObtenerPorId(id);
        if (existente == null)
        {
            return false;
        }

        return _repository.Eliminar(id);
    }

    private void ValidarCategoria(Categoria Categoria)
    {
        if (string.IsNullOrWhiteSpace(Categoria.Nombre))
        {
            throw new ArgumentException("El nombre de la Categoria es obligatorio");
        }
    }
}