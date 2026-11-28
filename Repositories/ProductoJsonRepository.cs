using Microsoft.AspNetCore.Hosting;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public class ProductoJsonRepository : JsonRepositoryBase<Producto>, IProductoRepository
{
    public ProductoJsonRepository(IWebHostEnvironment environment)
        : base(
            environment,
            "productos.json",
            producto => new Producto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                ImagenUrl = producto.ImagenUrl,
                CategoriaId = producto.CategoriaId,
                Activo = producto.Activo,
                FechaAlta = producto.FechaAlta,
                Categoria = null
            },
            producto => producto.Id,
            (producto, id) => producto.Id = id)
    {
    }

    public List<Producto> ObtenerTodos(
        int? categoriaId = null,
        string? nombre = null)
    {
        IEnumerable<Producto> consulta = ObtenerTodosBase()
            .Where(producto => producto.Activo);

        if (categoriaId.HasValue)
        {
            consulta = consulta.Where(producto =>
                producto.CategoriaId == categoriaId.Value);
        }

        if (!string.IsNullOrWhiteSpace(nombre))
        {
            var termino = nombre.Trim();

            consulta = consulta.Where(producto =>
                producto.Nombre.Contains(
                    termino,
                    StringComparison.OrdinalIgnoreCase));
        }

        return consulta
            .OrderBy(producto => producto.Nombre)
            .ToList();
    }

    public Producto? ObtenerPorId(int id) => ObtenerPorIdBase(id);

    public Producto Agregar(Producto nuevoProducto)
    {
        nuevoProducto.Categoria = null;
        return AgregarBase(nuevoProducto);
    }

    public bool Actualizar(int id, Producto producto)
    {
        return ActualizarBase(id, existente =>
        {
            existente.Nombre = producto.Nombre;
            existente.Descripcion = producto.Descripcion;
            existente.Precio = producto.Precio;
            existente.Stock = producto.Stock;
            existente.ImagenUrl = producto.ImagenUrl;
            existente.CategoriaId = producto.CategoriaId;
            existente.Activo = producto.Activo;
            existente.FechaAlta = producto.FechaAlta;
        });
    }

    // Se conserva el comportamiento de PostgreSQL: baja lógica.
    public bool Eliminar(int id)
    {
        return ActualizarBase(id, existente =>
        {
            existente.Activo = false;
        });
    }
}
