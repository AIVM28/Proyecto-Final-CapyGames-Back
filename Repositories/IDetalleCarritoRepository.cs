using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public interface IDetalleCarritoRepository
{
    List<DetalleCarrito> ObtenerTodas();
    DetalleCarrito? ObtenerPorId(int id);
    DetalleCarrito Agregar(DetalleCarrito nuevoDetalleCarrito);
    bool Actualizar(int id, DetalleCarrito detalleCarrito);
    bool Eliminar(int id);
}