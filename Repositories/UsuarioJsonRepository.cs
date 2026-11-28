using Microsoft.AspNetCore.Hosting;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public class UsuarioJsonRepository : JsonRepositoryBase<Usuario>, IUsuarioRepository
{
    public UsuarioJsonRepository(IWebHostEnvironment environment)
        : base(
            environment,
            "usuarios.json",
            usuario => new Usuario
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellidos,
                Email = usuario.Email,
                PasswordHash = usuario.PasswordHash,
                Telefono = usuario.Telefono,
                Rol = usuario.Rol,
                FechaRegistro = usuario.FechaRegistro,
                Activo = usuario.Activo
            },
            usuario => usuario.Id,
            (usuario, id) => usuario.Id = id)
    {
    }

    public List<Usuario> ObtenerTodos() => ObtenerTodosBase();

    public Usuario? ObtenerPorId(int id) => ObtenerPorIdBase(id);

    public Usuario? ObtenerPorEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        var emailNormalizado = email.Trim();
        return BuscarBase(usuario => string.Equals(
            usuario.Email,
            emailNormalizado,
            StringComparison.OrdinalIgnoreCase));
    }

    public Usuario Agregar(Usuario nuevoUsuario)
    {
        if (ObtenerPorEmail(nuevoUsuario.Email) is not null)
        {
            throw new InvalidOperationException(
                "Ya existe un usuario registrado con ese correo electrónico.");
        }

        return AgregarBase(nuevoUsuario);
    }

    public bool Actualizar(int id, Usuario usuario)
    {
        var usuarioConMismoCorreo = ObtenerPorEmail(usuario.Email);

        if (usuarioConMismoCorreo is not null && usuarioConMismoCorreo.Id != id)
        {
            throw new InvalidOperationException(
                "El correo electrónico ya está registrado por otro usuario.");
        }

        return ActualizarBase(id, existente =>
        {
            existente.Nombre = usuario.Nombre;
            existente.Apellidos = usuario.Apellidos;
            existente.Email = usuario.Email;
            existente.PasswordHash = usuario.PasswordHash;
            existente.Telefono = usuario.Telefono;
            existente.Rol = usuario.Rol;
            existente.FechaRegistro = usuario.FechaRegistro;
            existente.Activo = usuario.Activo;
        });
    }

    public bool Eliminar(int id) => EliminarBase(id);
}
