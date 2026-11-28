
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

        // Tablas de la base de datos
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Direccion> Direcciones => Set<Direccion>();
        public DbSet<Carrito> Carritos => Set<Carrito>();
        public DbSet<DetalleCarrito> DetalleCarritos => Set<DetalleCarrito>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<DetallePedido> DetallePedidos => Set<DetallePedido>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // USUARIO
            // ==========================================

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Apellidos)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.Property(u => u.PasswordHash)
                    .IsRequired();

                entity.Property(u => u.Telefono)
                    .HasMaxLength(20);

                entity.Property(u => u.Rol)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(u => u.FechaRegistro)
                    .IsRequired();

                entity.Property(u => u.Activo)
                    .HasDefaultValue(true);
            });

            // ==========================================
            // CATEGORIA
            // ==========================================

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Descripcion)
                    .IsRequired();

                entity.Property(c => c.Activa)
                    .HasDefaultValue(true);
            });

            // ==========================================
            // PRODUCTO
            // ==========================================

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Nombre)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(p => p.Descripcion)
                    .IsRequired();

                entity.Property(p => p.Precio)
                    .HasPrecision(12, 2);

                entity.Property(p => p.ImagenUrl)
                    .HasMaxLength(2048);

                entity.Property(p => p.Activo)
                    .HasDefaultValue(true);

                entity.HasOne(p => p.Categoria)
                    .WithMany()
                    .HasForeignKey(p => p.CategoriaId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ==========================================
            // DIRECCION
            // ==========================================

            modelBuilder.Entity<Direccion>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Calle)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(d => d.NumeroExterior)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(d => d.NumeroInterior)
                    .HasMaxLength(20);

                entity.Property(d => d.Colonia)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(d => d.Ciudad)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(d => d.Estado)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(d => d.CodigoPostal)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(d => d.Referencias)
                    .IsRequired();

                entity.Property(d => d.Predeterminada)
                    .HasDefaultValue(false);

                entity.HasOne(d => d.Usuario)
                    .WithMany()
                    .HasForeignKey(d => d.UsuarioId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ==========================================
            // CARRITO
            // ==========================================

            modelBuilder.Entity<Carrito>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.FechaActualizacion)
                    .IsRequired();

                entity.HasOne(c => c.Usuario)
                    .WithOne()
                    .HasForeignKey<Carrito>(c => c.UsuarioId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ==========================================
            // DETALLE CARRITO
            // ==========================================


            modelBuilder.Entity<DetalleCarrito>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Cantidad)
                    .IsRequired();

                entity.HasIndex(d => new { d.CarritoId, d.ProductoId })
                    .IsUnique();

                entity.HasOne(d => d.Carrito)
                    .WithMany(c => c.DetallesCarrito)
                    .HasForeignKey(d => d.CarritoId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Producto)
                    .WithMany()
                    .HasForeignKey(d => d.ProductoId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // ==========================================
            // PEDIDO
            // ==========================================

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.MetodoPago)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(p => p.Estado)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDefaultValue("Pendiente");

                entity.Property(p => p.Subtotal)
                    .HasPrecision(12, 2);

                entity.Property(p => p.CostoEnvio)
                    .HasPrecision(12, 2);

                entity.Property(p => p.Total)
                    .HasPrecision(12, 2);

                entity.HasOne(p => p.Usuario)
                    .WithMany()
                    .HasForeignKey(p => p.UsuarioId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Direccion)
                    .WithMany()
                    .HasForeignKey(p => p.DireccionId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ==========================================
            // DETALLE PEDIDO
            // ==========================================

            modelBuilder.Entity<DetallePedido>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Cantidad)
                    .IsRequired();

                entity.Property(d => d.PrecioUnitario)
                    .HasPrecision(12, 2);

                entity.Property(d => d.Subtotal)
                    .HasPrecision(12, 2);

                entity.HasOne(d => d.Pedido)
                    .WithMany(p => p.DetallesPedido)
                    .HasForeignKey(d => d.PedidoId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Producto)
                    .WithMany()
                    .HasForeignKey(d => d.ProductoId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
