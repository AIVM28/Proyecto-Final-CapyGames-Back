using Microsoft.EntityFrameworkCore;
using Proyecto_Final_CapyGames_Back.Data;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public class PedidoPostgreSqlRepository : IPedidoRepository
{
    private readonly PostgreSqlDbContext _context;

    public PedidoPostgreSqlRepository(PostgreSqlDbContext context)
    {
        _context = context;
    }

    public List<Pedido> ObtenerPorUsuario(int usuarioId)
    {
        return _context.Pedidos
            .AsNoTracking()
            .Include(p => p.DetallesPedido)
                .ThenInclude(d => d.Producto)
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.Fecha)
            .ToList();
    }

    public Pedido? ObtenerPorIdUsuario(int id, int usuarioId)
    {
        return _context.Pedidos
            .Include(p => p.DetallesPedido)
                .ThenInclude(d => d.Producto)
            .FirstOrDefault(p =>
                p.Id == id && p.UsuarioId == usuarioId
            );
    }

    public List<Pedido> ObtenerTodos(string? estado = null)
    {
        var consulta = _context.Pedidos
            .AsNoTracking()
            .Include(p => p.DetallesPedido)
                .ThenInclude(d => d.Producto)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(estado))
        {
            consulta = consulta.Where(p => p.Estado == estado);
        }

        return consulta
            .OrderByDescending(p => p.Fecha)
            .ToList();
    }

    public Pedido? ObtenerPorId(int id)
    {
        return _context.Pedidos
            .Include(p => p.DetallesPedido)
                .ThenInclude(d => d.Producto)
            .FirstOrDefault(p => p.Id == id);
    }

    public Pedido Agregar(Pedido nuevoPedido)
    {
        nuevoPedido.Id = 0;

        _context.Pedidos.Add(nuevoPedido);
        _context.SaveChanges();

        return nuevoPedido;
    }

    public void GuardarCambios()
    {
        _context.SaveChanges();
    }
}
