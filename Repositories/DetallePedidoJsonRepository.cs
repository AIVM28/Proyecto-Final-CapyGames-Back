using Microsoft.AspNetCore.Hosting;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public class DetallePedidoJsonRepository : JsonRepositoryBase<DetallePedido>, IDetallePedidoRepository
{
    public DetallePedidoJsonRepository(IWebHostEnvironment environment)
        : base(
            environment,
            "detalles-pedido.json",
            detalle => new DetallePedido
            {
                Id = detalle.Id,
                PedidoId = detalle.PedidoId,
                ProductoId = detalle.ProductoId,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = detalle.PrecioUnitario,
                Subtotal = detalle.Subtotal,
                Pedido = null,
                Producto = null
            },
            detalle => detalle.Id,
            (detalle, id) => detalle.Id = id)
    {
    }

    public List<DetallePedido> ObtenerTodos() => ObtenerTodosBase();

    public DetallePedido? ObtenerPorId(int id) => ObtenerPorIdBase(id);

    public DetallePedido Agregar(DetallePedido nuevoDetallePedido)
        => AgregarBase(nuevoDetallePedido);

    public bool Actualizar(int id, DetallePedido detallePedido)
    {
        return ActualizarBase(id, existente =>
        {
            existente.PedidoId = detallePedido.PedidoId;
            existente.ProductoId = detallePedido.ProductoId;
            existente.Cantidad = detallePedido.Cantidad;
            existente.PrecioUnitario = detallePedido.PrecioUnitario;
            existente.Subtotal = detallePedido.Subtotal;
        });
    }

    public bool Eliminar(int id) => EliminarBase(id);
}
