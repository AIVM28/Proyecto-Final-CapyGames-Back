using Proyecto_Final_CapyGames_Back.Data;
using Proyecto_Final_CapyGames_Back.Models;
namespace Proyecto_Final_CapyGames_Back.Repositories;
using Microsoft.EntityFrameworkCore;
public class ProductoPostgreSqlRepository : IProductoRepository
{
   private readonly PostgreSqlDbContext _context;
   public ProductoPostgreSqlRepository(
       PostgreSqlDbContext context)
   {
       _context = context;
   }

    public List<Producto> ObtenerTodos(
        int? categoriaId = null,
        string? buscar = null)
    {
        var consulta = _context.Productos
            .AsNoTracking()
            .Where(p => p.Activo);

        // Filtrar por categoría
        if (categoriaId.HasValue)
        {
            consulta = consulta.Where(
                p => p.CategoriaId == categoriaId.Value
            );
        }

        // Buscar por nombre
        if (!string.IsNullOrWhiteSpace(buscar))
        {
            string termino = buscar.Trim();

            consulta = consulta.Where(
                p => EF.Functions.ILike(p.Nombre, $"%{termino}%")
            );
        }

        return consulta.OrderBy(p => p.Nombre).ToList();
    }

   public Producto? ObtenerPorId(int id)
   {
       return _context.Productos.Find(id);
   }
    public Producto Agregar(Producto producto)
    {
        producto.Id = 0;

        // La relación se establece mediante CategoriaId
        producto.Categoria = null;

        _context.Productos.Add(producto);
        _context.SaveChanges();

        return producto;
    }

    public bool Actualizar(int id, Producto producto)
    {
        var existente = _context.Productos.Find(id);

        if (existente == null)
        {
            return false;
        }

        existente.Nombre = producto.Nombre;
        existente.Descripcion = producto.Descripcion;
        existente.Precio = producto.Precio;
        existente.Stock = producto.Stock;
        existente.ImagenUrl = producto.ImagenUrl;
        existente.CategoriaId = producto.CategoriaId;
        existente.Activo = producto.Activo;

        _context.SaveChanges();

        return true;
    }


    public bool Eliminar(int id)
    {
        var producto = _context.Productos.Find(id);

        if (producto == null)
        {
            return false;
        }

        producto.Activo = false;

        _context.SaveChanges();

        return true;
    }

}