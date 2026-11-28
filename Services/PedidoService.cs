
using Microsoft.EntityFrameworkCore;

using Proyecto_Final_CapyGames_Back.Data;
using Proyecto_Final_CapyGames_Back.DTOs;
using Proyecto_Final_CapyGames_Back.Models;
using Proyecto_Final_CapyGames_Back.Repositories;

namespace Proyecto_Final_CapyGames_Back.Services;

public class PedidoService : IPedidoService
{
    private readonly PostgreSqlDbContext _context;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly ICarritoRepository _carritoRepository;
    private readonly IDetalleCarritoService _detalleCarritoService;
    private readonly IProductoRepository _productoRepository;
    private readonly IDireccionRepository _direccionRepository;

    public PedidoService(
        PostgreSqlDbContext context,
        IPedidoRepository pedidoRepository,
        ICarritoRepository carritoRepository,
        IDetalleCarritoService detalleCarritoService,
        IProductoRepository productoRepository,
        IDireccionRepository direccionRepository)
    {
        _context = context;
        _pedidoRepository = pedidoRepository;
        _carritoRepository = carritoRepository;
        _detalleCarritoService = detalleCarritoService;
        _productoRepository = productoRepository;
        _direccionRepository = direccionRepository;
    }

    // Crear pedido y procesar checkout
    public PedidoRespuestaDto CrearPedido(
        int usuarioId,
        CrearPedidoDto solicitud)
    {
        string metodoPago = NormalizarMetodoPago(solicitud.MetodoPago);

        using var transaccion = _context.Database.BeginTransaction();

        try
        {
            var direccion = _direccionRepository.ObtenerPorId(
                solicitud.DireccionId,
                usuarioId
            );

            if (direccion == null)
            {
                throw new KeyNotFoundException(
                    "La dirección no existe o no pertenece al usuario."
                );
            }

            var carrito = _carritoRepository.ObtenerPorUsuario(usuarioId);

            if (carrito == null || !carrito.DetallesCarrito.Any())
            {
                throw new InvalidOperationException(
                    "El carrito está vacío."
                );
            }

            decimal subtotal = 0;

            var detallesPedido = new List<DetallePedido>();

            foreach (var item in carrito.DetallesCarrito)
            {
                var producto = item.Producto
                    ?? _productoRepository.ObtenerPorId(item.ProductoId);

                if (producto == null || !producto.Activo)
                {
                    throw new InvalidOperationException(
                        "Uno de los productos ya no está disponible."
                    );
                }

                if (producto.Stock < item.Cantidad)
                {
                    throw new InvalidOperationException(
                        $"Stock insuficiente para {producto.Nombre}."
                    );
                }

                decimal precio = producto.Precio;
                decimal importe = precio * item.Cantidad;

                subtotal += importe;

                producto.Stock -= item.Cantidad;

                detallesPedido.Add(new DetallePedido
                {
                    ProductoId = producto.Id,
                    Producto = producto,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = precio,
                    Subtotal = importe
                });
            }

            decimal costoEnvio = subtotal < 999m ? 99m : 0m;

            var pedido = new Pedido
            {
                UsuarioId = usuarioId,
                DireccionId = direccion.Id,
                Fecha = DateTime.UtcNow,
                Subtotal = subtotal,
                CostoEnvio = costoEnvio,
                Total = subtotal + costoEnvio,
                MetodoPago = metodoPago,
                Estado = "Pendiente",
                DetallesPedido = detallesPedido
            };

            _pedidoRepository.Agregar(pedido);

            pedido.Folio = pedido.Id;

            _pedidoRepository.GuardarCambios();

            foreach (var detalle in carrito.DetallesCarrito.ToList())
            {
                bool eliminado = _detalleCarritoService.Eliminar(
                    detalle.Id,
                    carrito.Id
                );

                if (!eliminado)
                {
                    throw new InvalidOperationException(
                        "No se pudo vaciar el carrito."
                    );
                }
            }

            _carritoRepository.ActualizarFecha(
                carrito.Id,
                DateTime.UtcNow
            );

            transaccion.Commit();

            var pedidoGuardado = _pedidoRepository.ObtenerPorId(pedido.Id);

            return CrearRespuesta(pedidoGuardado ?? pedido);
        }
        catch
        {
            transaccion.Rollback();
            throw;
        }
    }

    // Consultar pedidos del usuario
    public List<PedidoRespuestaDto> ObtenerPorUsuario(int usuarioId)
    {
        return _pedidoRepository.ObtenerPorUsuario(usuarioId)
            .Select(CrearRespuesta)
            .ToList();
    }

    // Consultar detalle de un pedido del usuario
    public PedidoRespuestaDto? ObtenerDetalle(int id, int usuarioId)
    {
        var pedido = _pedidoRepository.ObtenerPorIdUsuario(id, usuarioId);

        return pedido == null ? null : CrearRespuesta(pedido);
    }

    // Cancelar pedido pendiente del cliente
    public bool Cancelar(int id, int usuarioId)
    {
        using var transaccion = _context.Database.BeginTransaction();

        try
        {
            var pedido = _pedidoRepository.ObtenerPorIdUsuario(
                id,
                usuarioId
            );

            if (pedido == null)
            {
                return false;
            }

            if (pedido.Estado != "Pendiente")
            {
                throw new InvalidOperationException(
                    "Solamente se pueden cancelar pedidos pendientes."
                );
            }

            RestaurarInventario(pedido);

            pedido.Estado = "Cancelado";

            _pedidoRepository.GuardarCambios();

            transaccion.Commit();

            return true;
        }
        catch
        {
            transaccion.Rollback();
            throw;
        }
    }

    // Consultar pedidos para administración
    public List<PedidoRespuestaDto> ObtenerTodosAdmin(string? estado)
    {
        string? estadoNormalizado = string.IsNullOrWhiteSpace(estado)
            ? null
            : NormalizarEstado(estado);

        return _pedidoRepository.ObtenerTodos(estadoNormalizado)
            .Select(CrearRespuesta)
            .ToList();
    }

    // Actualizar estado administrativo
    public bool ActualizarEstadoAdmin(int id, string estado)
    {
        string nuevoEstado = NormalizarEstado(estado);

        using var transaccion = _context.Database.BeginTransaction();

        try
        {
            var pedido = _pedidoRepository.ObtenerPorId(id);

            if (pedido == null)
            {
                return false;
            }

            bool transicionValida = pedido.Estado switch
            {
                "Pendiente" => nuevoEstado is "Pagado" or "Cancelado",
                "Pagado" => nuevoEstado is "Enviado" or "Cancelado",
                "Enviado" => nuevoEstado == "Entregado",
                _ => false
            };

            if (!transicionValida)
            {
                throw new InvalidOperationException(
                    $"No se puede cambiar de {pedido.Estado} a {nuevoEstado}."
                );
            }

            if (nuevoEstado == "Cancelado")
            {
                RestaurarInventario(pedido);
            }

            pedido.Estado = nuevoEstado;

            _pedidoRepository.GuardarCambios();

            transaccion.Commit();

            return true;
        }
        catch
        {
            transaccion.Rollback();
            throw;
        }
    }

    // Obtener indicadores administrativos
    public ResumenAdminDto ObtenerResumenAdmin()
    {
        var pedidos = _pedidoRepository.ObtenerTodos();
        var productos = _productoRepository.ObtenerTodos();

        decimal ventas = pedidos
            .Where(p => p.Estado is "Pagado" or "Enviado" or "Entregado")
            .Sum(p => p.Total);

        return new ResumenAdminDto
        {
            TotalPedidos = pedidos.Count,
            VentasTotales = ventas,
            ProductosStockBajo = productos.Count(p => p.Stock <= 5)
        };
    }

    // Devolver inventario de un pedido cancelado
    private void RestaurarInventario(Pedido pedido)
    {
        foreach (var detalle in pedido.DetallesPedido)
        {
            var producto = detalle.Producto
                ?? _productoRepository.ObtenerPorId(detalle.ProductoId);

            if (producto == null)
            {
                throw new InvalidOperationException(
                    "No se encontró un producto del pedido."
                );
            }

            producto.Stock += detalle.Cantidad;

            _productoRepository.Actualizar(
                producto.Id,
                producto
            );
        }
    }

    // Calcular respuesta pública del pedido
    private PedidoRespuestaDto CrearRespuesta(Pedido pedido)
    {
        return new PedidoRespuestaDto
        {
            Id = pedido.Id,
            Folio = pedido.Folio,
            DireccionId = pedido.DireccionId,
            Fecha = pedido.Fecha,
            Subtotal = pedido.Subtotal,
            CostoEnvio = pedido.CostoEnvio,
            Total = pedido.Total,
            MetodoPago = pedido.MetodoPago,
            Estado = pedido.Estado,
            Detalles = pedido.DetallesPedido.Select(d =>
                new DetallePedidoRespuestaDto
                {
                    ProductoId = d.ProductoId,
                    NombreProducto = d.Producto?.Nombre ?? "Producto",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList()
        };
    }

    // Normalizar los métodos de pago permitidos
    private string NormalizarMetodoPago(string metodoPago)
    {
        return metodoPago.Trim().ToLowerInvariant() switch
        {
            "tarjeta" => "Tarjeta",
            "transferencia" => "Transferencia",
            "pago contra entrega" => "Pago contra entrega",
            _ => throw new ArgumentException(
                "Método de pago no válido."
            )
        };
    }

    // Validar los estados de pedido
    private string NormalizarEstado(string estado)
    {
        return estado.Trim().ToLowerInvariant() switch
        {
            "pendiente" => "Pendiente",
            "pagado" => "Pagado",
            "enviado" => "Enviado",
            "entregado" => "Entregado",
            "cancelado" => "Cancelado",
            _ => throw new ArgumentException(
                "Estado de pedido no válido."
            )
        };
    }
}
