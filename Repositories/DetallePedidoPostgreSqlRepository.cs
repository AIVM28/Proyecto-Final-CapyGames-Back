using Proyecto_Final_CapyGames_Back.Data;
using Proyecto_Final_CapyGames_Back.Models;
namespace Proyecto_Final_CapyGames_Back.Repositories;
public class DetallePedidoPostgreSqlRepository : IDetallePedidoRepository
{
   private readonly PostgreSqlDbContext _context;
   public DetallePedidoPostgreSqlRepository(
       PostgreSqlDbContext context)
   {
       _context = context;
   }
   public List<DetallePedido> ObtenerTodos()
   {
       return _context.DetallePedidos.ToList();
   }
   public DetallePedido? ObtenerPorId(int id)
   {
       return _context.DetallePedidos.Find(id);
   }
   public DetallePedido Agregar(DetallePedido DetallePedido)
   {
       DetallePedido.Id = 0;
       _context.DetallePedidos.Add(DetallePedido);
       _context.SaveChanges();
       return DetallePedido;
   }
   public bool Actualizar(int id, DetallePedido DetallePedido)
   {
       var existente = _context.DetallePedidos.Find(id);
       if (existente == null)
           return false;
       existente.PedidoId = DetallePedido.PedidoId;
       existente.ProductoId = DetallePedido.ProductoId;
       existente.Cantidad = DetallePedido.Cantidad;
       existente.PrecioUnitario = DetallePedido.PrecioUnitario;
       existente.Subtotal = DetallePedido.Subtotal;
       _context.SaveChanges();
       return true;
   }
   public bool Eliminar(int id)
   {
       var detallepedido = _context.DetallePedidos.Find(id);
       if (detallepedido == null)
           return false;
       _context.DetallePedidos.Remove(detallepedido);
       _context.SaveChanges();
       return true;
   }
}