using Microsoft.AspNetCore.Hosting;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public class DetalleCarritoJsonRepository : JsonRepositoryBase<DetalleCarrito>, IDetalleCarritoRepository
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

    public List<DetalleCarrito> ObtenerTodas() => ObtenerTodosBase();

    public DetalleCarrito? ObtenerPorId(int id) => ObtenerPorIdBase(id);

    public DetalleCarrito Agregar(DetalleCarrito nuevoDetalleCarrito)
        => AgregarBase(nuevoDetalleCarrito);

    public bool Actualizar(int id, DetalleCarrito detalleCarrito)
    {
        return ActualizarBase(id, existente =>
        {
            existente.CarritoId = detalleCarrito.CarritoId;
            existente.ProductoId = detalleCarrito.ProductoId;
            existente.Cantidad = detalleCarrito.Cantidad;
        });
    }

    public bool Eliminar(int id) => EliminarBase(id);
}
