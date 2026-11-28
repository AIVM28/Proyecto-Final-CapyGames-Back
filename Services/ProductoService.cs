
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

    // CONSULTAR PRODUCTOS
    public List<Producto> ObtenerTodos(
        int? categoriaId = null,
        string? buscar = null)
    {
        return _repository.ObtenerTodos(categoriaId, buscar);
    }

    // CONSULTAR PRODUCTO POR ID
    public Producto? ObtenerPorId(int id)
    {
        var producto = _repository.ObtenerPorId(id);

        if (producto == null || !producto.Activo)
        {
            return null;
        }

        return producto;
    }

    // AGREGAR PRODUCTO
    public Producto Agregar(Producto nuevoProducto)
    {
        ValidarProducto(nuevoProducto);

        nuevoProducto.Id = 0;
        nuevoProducto.Activo = true;
        nuevoProducto.FechaAlta = DateTime.UtcNow;

        return _repository.Agregar(nuevoProducto);
    }


    // ACTUALIZAR PRODUCTO
    public bool Actualizar(int id, Producto producto)
    {
        var existente = _repository.ObtenerPorId(id);

        if (existente == null)
        {
            return false;
        }

        ValidarProducto(producto);

        // Conservar datos administrados por el servidor
        producto.Id = id;
        producto.FechaAlta = existente.FechaAlta;
        producto.Activo = existente.Activo;

        return _repository.Actualizar(id, producto);
    }

    // ==========================================
    // DESACTIVAR PRODUCTO
    // ==========================================

    public bool Eliminar(int id)
    {
        var existente = _repository.ObtenerPorId(id);

        if (existente == null || !existente.Activo)
        {
            return false;
        }

        existente.Activo = false;

        return _repository.Actualizar(id, existente);
    }
    // VALIDACIONES
    private void ValidarProducto(Producto producto)
    {
        if (string.IsNullOrWhiteSpace(producto.Nombre))
        {
            throw new ArgumentException(
                "El nombre del producto es obligatorio."
            );
        }

        if (producto.Precio <= 0)
        {
            throw new ArgumentException(
                "El precio debe ser mayor a cero."
            );
        }

        if (producto.Stock < 0)
        {
            throw new ArgumentException(
                "El stock no puede ser negativo."
            );
        }

        if (producto.CategoriaId <= 0)
        {
            throw new ArgumentException(
                "Debe seleccionar una categoría válida."
            );
        }
    }
}
