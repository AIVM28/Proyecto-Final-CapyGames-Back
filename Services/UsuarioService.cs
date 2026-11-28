using Proyecto_Final_CapyGames_Back.Models;
using Proyecto_Final_CapyGames_Back.Repositories;

namespace Proyecto_Final_CapyGames_Back.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public List<Usuario> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public Usuario? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public Usuario Agregar(Usuario nuevoUsuario)
    {
        ValidarUsuario(nuevoUsuario);

        return _repository.Agregar(nuevoUsuario);
    }

    public bool Actualizar(int id, Usuario usuario)
    {
        var existente = _repository.ObtenerPorId(id);
        if (existente == null)
        {
            return false;
        }
        ValidarUsuario(usuario);

        return _repository.Actualizar(id, usuario);
    }

    public bool Eliminar(int id)
    {
        var existente = _repository.ObtenerPorId(id);
        if (existente == null)
        {
            return false;
        }

        return _repository.Eliminar(id);
    }

    private void ValidarUsuario(Usuario usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario.Nombre))
        {
            throw new ArgumentException("El nombre de usuario es obligatorio");
        }
        if (string.IsNullOrWhiteSpace(usuario.Email))
        {
            throw new ArgumentException("El correo del usuario es obligatorio");
        }        
    }
}