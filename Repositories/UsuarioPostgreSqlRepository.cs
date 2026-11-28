using Proyecto_Final_CapyGames_Back.Data;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public class UsuarioPostgreSqlRepository : IUsuarioRepository
{
    private readonly PostgreSqlDbContext _context;

    public UsuarioPostgreSqlRepository(PostgreSqlDbContext context)
    {
        _context = context;
    }

    public List<Usuario> ObtenerTodos()
    {
        return _context.Usuarios.ToList();
    }

    public Usuario? ObtenerPorId(int id)
    {
        return _context.Usuarios.Find(id);
    }

    public Usuario? ObtenerPorEmail(string email)
    {
        return _context.Usuarios.FirstOrDefault(u => u.Email == email);
    }

    public Usuario Agregar(Usuario usuario)
    {
        usuario.Id = 0;

        _context.Usuarios.Add(usuario);
        _context.SaveChanges();

        return usuario;
    }

    public bool Actualizar(int id, Usuario usuario)
    {
        var existente = _context.Usuarios.Find(id);

        if (existente == null)
            return false;

        existente.Nombre = usuario.Nombre;
        existente.Apellidos = usuario.Apellidos;
        existente.Email = usuario.Email;
        existente.PasswordHash = usuario.PasswordHash;
        existente.Telefono = usuario.Telefono;
        existente.Rol = usuario.Rol;
        existente.FechaRegistro = usuario.FechaRegistro;
        existente.Activo = usuario.Activo;

        _context.SaveChanges();

        return true;
    }

    public bool Eliminar(int id)
    {
        var usuario = _context.Usuarios.Find(id);

        if (usuario == null)
            return false;

        _context.Usuarios.Remove(usuario);
        _context.SaveChanges();

        return true;
    }
}
