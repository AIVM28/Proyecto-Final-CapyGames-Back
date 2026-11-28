using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Services;

public interface IDireccionService
{
    List<Direccion> ObtenerPorUsuario(int usuarioId);

    Direccion Agregar(int usuarioId, Direccion nuevaDireccion);

    bool Actualizar(int id, int usuarioId, Direccion direccion);

    bool Eliminar(int id, int usuarioId);
}
