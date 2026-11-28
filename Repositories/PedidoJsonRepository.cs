using Microsoft.AspNetCore.Hosting;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public class PedidoJsonRepository : JsonRepositoryBase<Pedido>, IPedidoRepository
{
    // Mantiene las mismas referencias durante el alcance de la petición para que
    // los cambios hechos por el servicio se guarden al llamar GuardarCambios().
    private List<Pedido>? _pedidosEnMemoria;

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
                DetallesPedido = pedido.DetallesPedido
                    .Select(detalle => new DetallePedido
                    {
                        Id = detalle.Id,
                        PedidoId = detalle.PedidoId,
                        ProductoId = detalle.ProductoId,
                        Cantidad = detalle.Cantidad,
                        PrecioUnitario = detalle.PrecioUnitario,
                        Subtotal = detalle.Subtotal,
                        Pedido = null,
                        Producto = null
                    })
                    .ToList()
            },
            pedido => pedido.Id,
            (pedido, id) => pedido.Id = id)
    {
    }

    private List<Pedido> ObtenerPedidosEnMemoria()
    {
        _pedidosEnMemoria ??= ObtenerTodosBase();
        return _pedidosEnMemoria;
    }

    public List<Pedido> ObtenerPorUsuario(int usuarioId)
    {
        return ObtenerPedidosEnMemoria()
            .Where(pedido => pedido.UsuarioId == usuarioId)
            .OrderByDescending(pedido => pedido.Fecha)
            .ToList();
    }

    public Pedido? ObtenerPorIdUsuario(int id, int usuarioId)
    {
        return ObtenerPedidosEnMemoria()
            .FirstOrDefault(pedido =>
                pedido.Id == id && pedido.UsuarioId == usuarioId);
    }

    public List<Pedido> ObtenerTodos(string? estado = null)
    {
        IEnumerable<Pedido> consulta = ObtenerPedidosEnMemoria();

        if (!string.IsNullOrWhiteSpace(estado))
        {
            consulta = consulta.Where(pedido =>
                string.Equals(
                    pedido.Estado,
                    estado.Trim(),
                    StringComparison.OrdinalIgnoreCase));
        }

        return consulta
            .OrderByDescending(pedido => pedido.Fecha)
            .ToList();
    }

    public Pedido? ObtenerPorId(int id)
    {
        return ObtenerPedidosEnMemoria()
            .FirstOrDefault(pedido => pedido.Id == id);
    }

    public Pedido Agregar(Pedido nuevoPedido)
    {
        nuevoPedido.Usuario = null;
        nuevoPedido.Direccion = null;

        foreach (var detalle in nuevoPedido.DetallesPedido)
        {
            detalle.Pedido = null;
            detalle.Producto = null;
        }

        var agregado = AgregarBase(nuevoPedido);

        // Recargar desde el archivo y devolver la misma instancia que quedará
        // rastreada en memoria para que Folio y cambios de Estado se persistan.
        _pedidosEnMemoria = ObtenerTodosBase();

        var pedidoGuardado = _pedidosEnMemoria.First(pedido => pedido.Id == agregado.Id);

        foreach (var detalle in pedidoGuardado.DetallesPedido)
        {
            detalle.PedidoId = pedidoGuardado.Id;
        }

        GuardarTodosBase(_pedidosEnMemoria);

        return pedidoGuardado;
    }

    public void GuardarCambios()
    {
        if (_pedidosEnMemoria is not null)
        {
            GuardarTodosBase(_pedidosEnMemoria);
        }
    }
}
