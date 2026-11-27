using Proyecto_Final_CapyGames_Back.Data;
using Proyecto_Final_CapyGames_Back.Models;
namespace Proyecto_Final_CapyGames_Back.Repositories;
public class ProductoPostgreSqlRepository : IProductoRepository
{
   private readonly PostgreSqlDbContext _context;
   public ProductoPostgreSqlRepository(
       PostgreSqlDbContext context)
   {
       _context = context;
   }
   public List<Producto> ObtenerTodos()
   {
       return _context.Productos.ToList();
   }
   public Producto? ObtenerPorId(int id)
   {
       return _context.Productos.Find(id);
   }
   public Producto Agregar(Producto Producto)
   {
       Producto.Id = 0;
       _context.Productos.Add(Producto);
       _context.SaveChanges();
       return Producto;
   }
   public bool Actualizar(int id, Producto Producto)
   {
       var existente = _context.Productos.Find(id);
       if (existente == null)
           return false;
       existente.Nombre = Producto.Nombre;
       existente.Descripcion = Producto.Descripcion;
       existente.Activo = Producto.Activo;
       _context.SaveChanges();
       return true;
   }
   public bool Eliminar(int id)
   {
       var Producto = _context.Productos.Find(id);
       if (Producto == null)
           return false;
       _context.Productos.Remove(Producto);
       _context.SaveChanges();
       return true;
   }
}