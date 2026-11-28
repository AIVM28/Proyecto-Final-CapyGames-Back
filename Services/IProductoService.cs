using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Services;

public interface IProductoService
{
    List<Producto> ObtenerTodos();
    Producto? ObtenerPorId(int id);
    Producto Agregar(Producto nuevoProducto);
    bool Actualizar(int id, Producto producto);
    bool Eliminar(int id);
}