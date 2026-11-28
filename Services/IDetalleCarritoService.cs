
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Services;

public interface IDetalleCarritoService
{
    DetalleCarrito? ObtenerPorProducto(int carritoId, int productoId);

    DetalleCarrito? ObtenerPorId(int id, int carritoId);

    DetalleCarrito Agregar(DetalleCarrito nuevoDetalle);

    bool ActualizarCantidad(int id, int carritoId, int cantidad);

    bool Eliminar(int id, int carritoId);
}
