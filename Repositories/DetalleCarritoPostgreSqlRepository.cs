
using Proyecto_Final_CapyGames_Back.Data;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public class DetalleCarritoPostgreSqlRepository : IDetalleCarritoRepository
{
    private readonly PostgreSqlDbContext _context;

    public DetalleCarritoPostgreSqlRepository(PostgreSqlDbContext context)
    {
        _context = context;
    }

    public DetalleCarrito? ObtenerPorProducto(int carritoId, int productoId)
    {
        return _context.DetalleCarritos.FirstOrDefault(
            d => d.CarritoId == carritoId &&
                 d.ProductoId == productoId
        );
    }

    public DetalleCarrito? ObtenerPorId(int id, int carritoId)
    {
        return _context.DetalleCarritos.FirstOrDefault(
            d => d.Id == id &&
                 d.CarritoId == carritoId
        );
    }

    public DetalleCarrito Agregar(DetalleCarrito nuevoDetalle)
    {
        nuevoDetalle.Id = 0;
        nuevoDetalle.Carrito = null;
        nuevoDetalle.Producto = null;

        _context.DetalleCarritos.Add(nuevoDetalle);
        _context.SaveChanges();

        return nuevoDetalle;
    }

    public bool ActualizarCantidad(int id, int carritoId, int cantidad)
    {
        var existente = ObtenerPorId(id, carritoId);

        if (existente == null)
        {
            return false;
        }

        existente.Cantidad = cantidad;

        _context.SaveChanges();

        return true;
    }

    public bool Eliminar(int id, int carritoId)
    {
        var existente = ObtenerPorId(id, carritoId);

        if (existente == null)
        {
            return false;
        }

        _context.DetalleCarritos.Remove(existente);
        _context.SaveChanges();

        return true;
    }
}
