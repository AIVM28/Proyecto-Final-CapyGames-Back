using Proyecto_Final_CapyGames_Back.Data;
using Proyecto_Final_CapyGames_Back.Models;
namespace Proyecto_Final_CapyGames_Back.Repositories;
public class PedidoPostgreSqlRepository : IPedidoRepository
{
   private readonly PostgreSqlDbContext _context;
   public PedidoPostgreSqlRepository(
       PostgreSqlDbContext context)
   {
       _context = context;
   }
   public List<Pedido> ObtenerTodos()
   {
       return _context.Pedidos.ToList();
   }
   public Pedido? ObtenerPorId(int id)
   {
       return _context.Pedidos.Find(id);
   }
   public Pedido Agregar(Pedido Pedido)
   {
       Pedido.Id = 0;
       _context.Pedidos.Add(Pedido);
       _context.SaveChanges();
       return Pedido;
   }
   public bool Actualizar(int id, Pedido Pedido)
   {
       var existente = _context.Pedidos.Find(id);
       if (existente == null)
           return false;
       existente.Folio = Pedido.Folio;
       existente.UsuarioId = Pedido.UsuarioId;
       existente.DireccionId = Pedido.DireccionId;
       existente.Fecha = Pedido.Fecha;
       existente.Subtotal = Pedido.Subtotal;
       existente.CostoEnvio = Pedido.CostoEnvio;
       existente.Total = Pedido.Total;
       existente.MetodoPago = Pedido.MetodoPago;
       existente.Estado = Pedido.Estado;
       _context.SaveChanges();
       return true;
   }
   public bool Eliminar(int id)
   {
       var pedido = _context.Pedidos.Find(id);
       if (pedido == null)
           return false;
       _context.Pedidos.Remove(pedido);
       _context.SaveChanges();
       return true;
   }
}