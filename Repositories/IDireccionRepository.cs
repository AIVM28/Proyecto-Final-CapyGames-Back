using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public interface IDireccionRepository
{
    List<Direccion> ObtenerTodas();
    Direccion? ObtenerPorId(int id);
    Direccion Agregar(Direccion nuevaDireccion);
    bool Actualizar(int id, Direccion direccion);
    bool Eliminar(int id);
}