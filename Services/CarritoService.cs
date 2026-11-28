
using Proyecto_Final_CapyGames_Back.Models;
using Proyecto_Final_CapyGames_Back.Repositories;

namespace Proyecto_Final_CapyGames_Back.Services;

public class CarritoService : ICarritoService
{
    private readonly ICarritoRepository _repository;

    public CarritoService(ICarritoRepository repository)
    {
        _repository = repository;
    }

    public List<Carrito> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public Carrito? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public Carrito Agregar(Carrito nuevoCarrito)
    {
        return _repository.Agregar(nuevoCarrito);
    }

    public bool Actualizar(int id, Carrito carrito)
    {
        return _repository.Actualizar(id, carrito);
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
