
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

    public DetalleCarrito? ObtenerPorProducto(int carritoId, int productoId)
    {
        return _repository.ObtenerPorProducto(carritoId, productoId);
    }

    public DetalleCarrito? ObtenerPorId(int id, int carritoId)
    {
        return _repository.ObtenerPorId(id, carritoId);
    }

    public DetalleCarrito Agregar(DetalleCarrito nuevoDetalle)
    {
        return _repository.Agregar(nuevoDetalle);
    }

    public bool ActualizarCantidad(int id, int carritoId, int cantidad)
    {
        return _repository.ActualizarCantidad(id, carritoId, cantidad);
    }

    public bool Eliminar(int id, int carritoId)
    {
        return _repository.Eliminar(id, carritoId);
    }
}

