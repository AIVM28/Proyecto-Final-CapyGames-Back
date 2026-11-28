using Proyecto_Final_CapyGames_Back.Data;
using Proyecto_Final_CapyGames_Back.Models;
namespace Proyecto_Final_CapyGames_Back.Repositories;
public class CategoriaPostgreSqlRepository : ICategoriaRepository
{
   private readonly PostgreSqlDbContext _context;
   public CategoriaPostgreSqlRepository(
       PostgreSqlDbContext context)
   {
       _context = context;
   }
   public List<Categoria> ObtenerTodas()
   {
       return _context.Categorias.ToList();
   }
   public Categoria? ObtenerPorId(int id)
   {
       return _context.Categorias.Find(id);
   }
   public Categoria Agregar(Categoria Categoria)
   {
       Categoria.Id = 0;
       _context.Categorias.Add(Categoria);
       _context.SaveChanges();
       return Categoria;
   }
   public bool Actualizar(int id, Categoria Categoria)
   {
       var existente = _context.Categorias.Find(id);
       if (existente == null)
           return false;
       existente.Nombre = Categoria.Nombre;
       existente.Descripcion = Categoria.Descripcion;
       existente.Activa = Categoria.Activa;
       _context.SaveChanges();
       return true;
   }
   public bool Eliminar(int id)
   {
       var categoria = _context.Categorias.Find(id);
       if (categoria == null)
           return false;
       _context.Categorias.Remove(categoria);
       _context.SaveChanges();
       return true;
   }
}