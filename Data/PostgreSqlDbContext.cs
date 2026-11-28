using Microsoft.EntityFrameworkCore;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Data
{
    public class PostgreSqlDbContext : DbContext
    {
        public PostgreSqlDbContext(
            DbContextOptions<PostgreSqlDbContext> options
        ) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<Direccion> Direcciones => Set<Direccion>();
        public DbSet<DetallePedido> DetallePedidos => Set<DetallePedido>();
        public DbSet<DetalleCarrito> DetalleCarritos => Set<DetalleCarrito>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Carrito> Carritos => Set<Carrito>();
        
    }
}