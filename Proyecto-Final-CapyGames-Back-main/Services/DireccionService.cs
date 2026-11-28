using Proyecto_Final_CapyGames_Back.Models;
using Proyecto_Final_CapyGames_Back.Repositories;

namespace Proyecto_Final_CapyGames_Back.Services;

public class DireccionService : IDireccionService
{
    private readonly IDireccionRepository _repository;

    public DireccionService(IDireccionRepository repository)
    {
        _repository = repository;
    }

    public List<Direccion> ObtenerTodas()
    {
        return _repository.ObtenerTodas();
    }

    public Direccion? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public Direccion Agregar(Direccion nuevoDireccion)
    {
        ValidarDireccion(nuevoDireccion);

        return _repository.Agregar(nuevoDireccion);
    }

    public bool Actualizar(int id, Direccion Direccion)
    {
        var existente = _repository.ObtenerPorId(id);
        if (existente == null)
        {
            return false;
        }
        ValidarDireccion(Direccion);

        return _repository.Actualizar(id, Direccion);
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

    private void ValidarDireccion(Direccion Direccion)
    {
        if (string.IsNullOrWhiteSpace(Direccion.Calle))
        {
            throw new ArgumentException("El nombre de la calle de la Direccion es obligatorio");
        }
    }
}