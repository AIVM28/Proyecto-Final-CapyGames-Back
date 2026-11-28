using Proyecto_Final_CapyGames_Back.Data;
using Proyecto_Final_CapyGames_Back.Models;
namespace Proyecto_Final_CapyGames_Back.Repositories;
public class DireccionPostgreSqlRepository : IDireccionRepository
{
   private readonly PostgreSqlDbContext _context;
   public DireccionPostgreSqlRepository(
       PostgreSqlDbContext context)
   {
       _context = context;
   }
   public List<Direccion> ObtenerTodas()
   {
       return _context.Direcciones.ToList();
   }
   public Direccion? ObtenerPorId(int id)
   {
       return _context.Direcciones.Find(id);
   }
   public Direccion Agregar(Direccion Direccion)
   {
       Direccion.Id = 0;
       _context.Direcciones.Add(Direccion);
       _context.SaveChanges();
       return Direccion;
   }
   public bool Actualizar(int id, Direccion Direccion)
   {
       var existente = _context.Direcciones.Find(id);
       if (existente == null)
           return false;
       existente.UsuarioId = Direccion.UsuarioId;
       existente.Calle = Direccion.Calle;
       existente.NumeroExterior = Direccion.NumeroExterior;
       existente.NumeroInterior = Direccion.NumeroInterior;
       existente.Colonia = Direccion.Colonia;
       existente.Ciudad = Direccion.Ciudad;
       existente.Estado = Direccion.Estado;
       existente.CodigoPostal = Direccion.CodigoPostal;
       existente.Referencias = Direccion.Referencias;
       existente.Predeterminada = Direccion.Predeterminada;
       _context.SaveChanges();
       return true;
   }
   public bool Eliminar(int id)
   {
       var direccion = _context.Direcciones.Find(id);
       if (direccion == null)
           return false;
       _context.Direcciones.Remove(direccion);
       _context.SaveChanges();
       return true;
   }
}