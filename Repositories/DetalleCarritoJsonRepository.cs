using Microsoft.AspNetCore.Hosting;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public class DetalleCarritoJsonRepository
    : JsonRepositoryBase<DetalleCarrito>, IDetalleCarritoRepository
{
    public DetalleCarritoJsonRepository(IWebHostEnvironment environment)
        : base(
            environment,
            "detalles-carrito.json",
            detalle => new DetalleCarrito
            {
                Id = detalle.Id,
                CarritoId = detalle.CarritoId,
                ProductoId = detalle.ProductoId,
                Cantidad = detalle.Cantidad,
                Carrito = null,
                Producto = null
            },
            detalle => detalle.Id,
            (detalle, id) => detalle.Id = id)
    {
    }

    public DetalleCarrito? ObtenerPorProducto(int carritoId, int productoId)
    {
        return BuscarBase(detalle =>
            detalle.CarritoId == carritoId &&
            detalle.ProductoId == productoId);
    }

    public DetalleCarrito? ObtenerPorId(int id, int carritoId)
    {
        return BuscarBase(detalle =>
            detalle.Id == id &&
            detalle.CarritoId == carritoId);
    }

    public DetalleCarrito Agregar(DetalleCarrito nuevoDetalle)
    {
        nuevoDetalle.Carrito = null;
        nuevoDetalle.Producto = null;
        return AgregarBase(nuevoDetalle);
    }

    public bool ActualizarCantidad(int id, int carritoId, int cantidad)
    {
        return ActualizarBase(
            id,
            detalle => detalle.CarritoId == carritoId,
            detalle => detalle.Cantidad = cantidad);
    }

    public bool Eliminar(int id, int carritoId)
    {
        return EliminarBase(
            id,
            detalle => detalle.CarritoId == carritoId);
    }
}
