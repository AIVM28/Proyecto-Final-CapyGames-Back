using Proyecto_Final_CapyGames_Back.Models;
using Proyecto_Final_CapyGames_Back.Repositories;

namespace Proyecto_Final_CapyGames_Back.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;

    public ProductoService(IProductoRepository repository)
    {
        _repository = repository;
    }

    public List<Producto> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public Producto? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public Producto Agregar(Producto nuevoProducto)
    {
        ValidarProducto(nuevoProducto);

        return _repository.Agregar(nuevoProducto);
    }

    public bool Actualizar(int id, Producto producto)
    {
        var existente = _repository.ObtenerPorId(id);
        if (existente == null)
        {
            return false;
        }
        ValidarProducto(producto);

        return _repository.Actualizar(id, producto);
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

    private void ValidarProducto(Producto Producto)
    {
        if (string.IsNullOrWhiteSpace(Producto.Nombre))
        {
            throw new ArgumentException("El nombre de Producto es obligatorio");
        }  
    }
}