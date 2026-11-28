using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Services;

public interface ICarritoService
{
    List<Carrito> ObtenerTodos();
    Carrito? ObtenerPorId(int id);
    Carrito Agregar(Carrito nuevoCarrito);
    bool Actualizar(int id, Carrito carrito);
    bool Eliminar(int id);
}