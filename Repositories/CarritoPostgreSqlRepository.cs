
using Microsoft.EntityFrameworkCore;
using Proyecto_Final_CapyGames_Back.Data;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public class CarritoPostgreSqlRepository : ICarritoRepository
{
    private readonly PostgreSqlDbContext _context;

    public CarritoPostgreSqlRepository(PostgreSqlDbContext context)
    {
        _context = context;
    }

    public Carrito? ObtenerPorUsuario(int usuarioId)
    {
        return _context.Carritos
            .Include(c => c.DetallesCarrito)
            .ThenInclude(d => d.Producto)
            .FirstOrDefault(c => c.UsuarioId == usuarioId);
    }

    public Carrito Agregar(Carrito nuevoCarrito)
    {
        nuevoCarrito.Id = 0;
        nuevoCarrito.Usuario = null;
        nuevoCarrito.DetallesCarrito.Clear();

        _context.Carritos.Add(nuevoCarrito);
        _context.SaveChanges();

        return nuevoCarrito;
    }

    public bool ActualizarFecha(int id, DateTime fechaActualizacion)
    {
        var carrito = _context.Carritos.Find(id);

        if (carrito == null)
        {
            return false;
        }

        carrito.FechaActualizacion = fechaActualizacion;

        _context.SaveChanges();

        return true;
    }
}
