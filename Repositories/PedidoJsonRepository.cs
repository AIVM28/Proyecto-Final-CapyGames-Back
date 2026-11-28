using Microsoft.AspNetCore.Hosting;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public class PedidoJsonRepository : JsonRepositoryBase<Pedido>, IPedidoRepository
{
    public PedidoJsonRepository(IWebHostEnvironment environment)
        : base(
            environment,
            "pedidos.json",
            pedido => new Pedido
            {
                Id = pedido.Id,
                Folio = pedido.Folio,
                UsuarioId = pedido.UsuarioId,
                DireccionId = pedido.DireccionId,
                Fecha = pedido.Fecha,
                Subtotal = pedido.Subtotal,
                CostoEnvio = pedido.CostoEnvio,
                Total = pedido.Total,
                MetodoPago = pedido.MetodoPago,
                Estado = pedido.Estado,
                Usuario = null,
                Direccion = null,
                DetallesPedido = new List<DetallePedido>()
            },
            pedido => pedido.Id,
            (pedido, id) => pedido.Id = id)
    {
    }

    public List<Pedido> ObtenerTodos() => ObtenerTodosBase();

    public Pedido? ObtenerPorId(int id) => ObtenerPorIdBase(id);

    public Pedido Agregar(Pedido nuevoPedido) => AgregarBase(nuevoPedido);

    public bool Actualizar(int id, Pedido pedido)
    {
        return ActualizarBase(id, existente =>
        {
            existente.Folio = pedido.Folio;
            existente.UsuarioId = pedido.UsuarioId;
            existente.DireccionId = pedido.DireccionId;
            existente.Fecha = pedido.Fecha;
            existente.Subtotal = pedido.Subtotal;
            existente.CostoEnvio = pedido.CostoEnvio;
            existente.Total = pedido.Total;
            existente.MetodoPago = pedido.MetodoPago;
            existente.Estado = pedido.Estado;
        });
    }

    public bool Eliminar(int id) => EliminarBase(id);
}
