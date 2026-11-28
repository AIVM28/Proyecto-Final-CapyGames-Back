using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Services;

public interface IDetalleCarritoService
{
    List<DetalleCarrito> ObtenerTodos();
    DetalleCarrito? ObtenerPorId(int id);
    DetalleCarrito Agregar(DetalleCarrito nuevoDetalleCarrito);
    bool Actualizar(int id, DetalleCarrito detalleCarrito);
    bool Eliminar(int id);
}