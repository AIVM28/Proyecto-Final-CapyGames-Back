using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public interface IUsuarioRepository
{
     List<Usuario> ObtenerTodos();
     Usuario? ObtenerPorId(int id);
     Usuario Agregar(Usuario nuevoUsuario);
     bool Actualizar(int id, Usuario usuario);
     bool Eliminar(int id);   
}