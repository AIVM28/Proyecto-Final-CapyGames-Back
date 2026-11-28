using Proyecto_Final_CapyGames_Back.Data;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public class DireccionPostgreSqlRepository : IDireccionRepository
{
    private readonly PostgreSqlDbContext _context;

    public DireccionPostgreSqlRepository(PostgreSqlDbContext context)
    {
        _context = context;
    }

    public List<Direccion> ObtenerPorUsuario(int usuarioId)
    {
        return _context.Direcciones
            .Where(d => d.UsuarioId == usuarioId)
            .ToList();
    }

    public Direccion? ObtenerPorId(int id, int usuarioId)
    {
        return _context.Direcciones
            .FirstOrDefault(d => d.Id == id && d.UsuarioId == usuarioId);
    }

    public Direccion Agregar(Direccion nuevaDireccion)
    {
        nuevaDireccion.Id = 0;
        nuevaDireccion.Usuario = null;

        _context.Direcciones.Add(nuevaDireccion);
        _context.SaveChanges();

        return nuevaDireccion;
    }

    public bool Actualizar(int id, int usuarioId, Direccion direccion)
    {
        var existente = _context.Direcciones
            .FirstOrDefault(d => d.Id == id && d.UsuarioId == usuarioId);

        if (existente == null)
        {
            return false;
        }

        existente.Calle = direccion.Calle;
        existente.NumeroExterior = direccion.NumeroExterior;
        existente.NumeroInterior = direccion.NumeroInterior;
        existente.Colonia = direccion.Colonia;
        existente.Ciudad = direccion.Ciudad;
        existente.Estado = direccion.Estado;
        existente.CodigoPostal = direccion.CodigoPostal;
        existente.Referencias = direccion.Referencias;
        existente.Predeterminada = direccion.Predeterminada;

        _context.SaveChanges();

        return true;
    }

    public bool Eliminar(int id, int usuarioId)
    {
        var direccion = _context.Direcciones
            .FirstOrDefault(d => d.Id == id && d.UsuarioId == usuarioId);

        if (direccion == null)
        {
            return false;
        }

        _context.Direcciones.Remove(direccion);
        _context.SaveChanges();

        return true;
    }
}
