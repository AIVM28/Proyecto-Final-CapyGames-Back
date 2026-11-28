
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public interface IDireccionRepository
{
    List<Direccion> ObtenerPorUsuario(int usuarioId);

    Direccion? ObtenerPorId(int id, int usuarioId);

    Direccion Agregar(Direccion nuevaDireccion);

    bool Actualizar(int id, int usuarioId, Direccion direccion);

    bool Eliminar(int id, int usuarioId);
}
