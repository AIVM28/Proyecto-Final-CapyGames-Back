using Proyecto_Final_CapyGames_Back.Data;
using Proyecto_Final_CapyGames_Back.Models;
namespace Proyecto_Final_CapyGames_Back.Repositories;
public class CarritoPostgreSqlRepository : ICarritoRepository
{
   private readonly PostgreSqlDbContext _context;
   public CarritoPostgreSqlRepository(
       PostgreSqlDbContext context)
   {
       _context = context;
   }
   public List<Carrito> ObtenerTodos()
   {
       return _context.Carritos.ToList();
   }
   public Carrito? ObtenerPorId(int id)
   {
       return _context.Carritos.Find(id);
   }
   public Carrito Agregar(Carrito Carrito)
   {
       Carrito.Id = 0;
       _context.Carritos.Add(Carrito);
       _context.SaveChanges();
       return Carrito;
   }
   public bool Actualizar(int id, Carrito Carrito)
   {
       var existente = _context.Carritos.Find(id);
       if (existente == null)
           return false;
       existente.UsuarioId = Carrito.UsuarioId;
       existente.FechaActualizacion = Carrito.FechaActualizacion;
       _context.SaveChanges();
       return true;
   }
   public bool Eliminar(int id)
   {
       var carrito = _context.Carritos.Find(id);
       if (carrito == null)
           return false;
       _context.Carritos.Remove(carrito);
       _context.SaveChanges();
       return true;
   }
}