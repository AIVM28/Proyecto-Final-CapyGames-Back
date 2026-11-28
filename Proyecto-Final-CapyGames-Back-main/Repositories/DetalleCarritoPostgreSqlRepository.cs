using Proyecto_Final_CapyGames_Back.Data;
using Proyecto_Final_CapyGames_Back.Models;
namespace Proyecto_Final_CapyGames_Back.Repositories;
public class DetalleCarritoPostgreSqlRepository : IDetalleCarritoRepository
{
   private readonly PostgreSqlDbContext _context;
   public DetalleCarritoPostgreSqlRepository(
       PostgreSqlDbContext context)
   {
       _context = context;
   }
   public List<DetalleCarrito> ObtenerTodos()
   {
       return _context.DetalleCarritos.ToList();
   }
   public DetalleCarrito? ObtenerPorId(int id)
   {
       return _context.DetalleCarritos.Find(id);
   }
   public DetalleCarrito Agregar(DetalleCarrito DetalleCarrito)
   {
       DetalleCarrito.Id = 0;
       _context.DetalleCarritos.Add(DetalleCarrito);
       _context.SaveChanges();
       return DetalleCarrito;
   }
   public bool Actualizar(int id, DetalleCarrito DetalleCarrito)
   {
       var existente = _context.DetalleCarritos.Find(id);
       if (existente == null)
           return false;
       existente.CarritoId = DetalleCarrito.CarritoId;
       existente.ProductoId = DetalleCarrito.ProductoId;
       existente.Cantidad = DetalleCarrito.Cantidad;
       _context.SaveChanges();
       return true;
   }
   public bool Eliminar(int id)
   {
       var detallecarrito = _context.DetalleCarritos.Find(id);
       if (detallecarrito == null)
           return false;
       _context.DetalleCarritos.Remove(detallecarrito);
       _context.SaveChanges();
       return true;
   }
}