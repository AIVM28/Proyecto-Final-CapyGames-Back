using Proyecto_Final_CapyGames_Back.Models;
using Proyecto_Final_CapyGames_Back.Repositories;

namespace Proyecto_Final_CapyGames_Back.Services;

public class DetalleCarritoService : IDetalleCarritoService
{
    private readonly IDetalleCarritoRepository _repository;

    public DetalleCarritoService(IDetalleCarritoRepository repository)
    {
        _repository = repository;
    }

    public List<DetalleCarrito> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public DetalleCarrito? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public DetalleCarrito Agregar(DetalleCarrito nuevoDetalleCarrito)
    {

        return _repository.Agregar(nuevoDetalleCarrito);
    }

    public bool Actualizar(int id, DetalleCarrito DetalleCarrito)
    {

        return _repository.Actualizar(id, DetalleCarrito);
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

}