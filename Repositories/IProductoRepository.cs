using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public interface IProductoRepository
{
    List<Producto> ObtenerTodos();
    Producto? ObtenerPorId(int id);
    Producto Agregar(Producto nuevoProducto);
    bool Actualizar(int id, Producto producto);
    bool Eliminar(int id);
}
