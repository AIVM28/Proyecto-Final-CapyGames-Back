using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Services;

public interface ICategoriaService
{
    List<Categoria> ObtenerTodas();
    Categoria? ObtenerPorId(int id);
    Categoria Agregar(Categoria nuevaCategoria);
    bool Actualizar(int id, Categoria categoria);
    bool Eliminar(int id);
}