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

    public List<Producto> ObtenerTodos() => ObtenerTodosBase();

    public Producto? ObtenerPorId(int id) => ObtenerPorIdBase(id);

    public Producto Agregar(Producto nuevoProducto) => AgregarBase(nuevoProducto);

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

    public bool Eliminar(int id) => EliminarBase(id);
}
