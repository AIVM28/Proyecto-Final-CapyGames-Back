
using Proyecto_Final_CapyGames_Back.DTOs;
using Proyecto_Final_CapyGames_Back.Models;
using Proyecto_Final_CapyGames_Back.Repositories;

namespace Proyecto_Final_CapyGames_Back.Services;

public class CarritoService : ICarritoService
{
    private readonly ICarritoRepository _carritoRepository;
    private readonly IDetalleCarritoService _detalleService;
    private readonly IProductoRepository _productoRepository;

    public CarritoService(
        ICarritoRepository carritoRepository,
        IDetalleCarritoService detalleService,
        IProductoRepository productoRepository)
    {
        _carritoRepository = carritoRepository;
        _detalleService = detalleService;
        _productoRepository = productoRepository;
    }

    // Consultar o crear el carrito del usuario
    public CarritoRespuestaDto ObtenerCarrito(int usuarioId)
    {
        var carrito = ObtenerOCrearCarrito(usuarioId);

        return CrearRespuesta(carrito);
    }

    // Agregar producto al carrito
    public CarritoRespuestaDto AgregarItem(
        int usuarioId,
        AgregarItemCarritoDto item)
    {
        if (item.Cantidad <= 0)
        {
            throw new ArgumentException(
                "La cantidad debe ser mayor a cero."
            );
        }

        var producto = _productoRepository.ObtenerPorId(item.ProductoId);

        if (producto == null)
        {
            throw new KeyNotFoundException("Producto no encontrado.");
        }

        if (!producto.Activo || producto.Stock <= 0)
        {
            throw new InvalidOperationException(
                "El producto no está disponible."
            );
        }

        var carrito = ObtenerOCrearCarrito(usuarioId);

        var existente = _detalleService.ObtenerPorProducto(
            carrito.Id,
            producto.Id
        );

        int cantidadTotal = item.Cantidad + (existente?.Cantidad ?? 0);

        if (cantidadTotal > producto.Stock)
        {
            throw new InvalidOperationException(
                "La cantidad solicitada supera el stock disponible."
            );
        }

        if (existente == null)
        {
            var nuevoDetalle = new DetalleCarrito
            {
                CarritoId = carrito.Id,
                ProductoId = producto.Id,
                Cantidad = item.Cantidad
            };

            _detalleService.Agregar(nuevoDetalle);
        }
        else
        {
            _detalleService.ActualizarCantidad(
                existente.Id,
                carrito.Id,
                cantidadTotal
            );
        }

        _carritoRepository.ActualizarFecha(
            carrito.Id,
            DateTime.UtcNow
        );

        return ObtenerCarrito(usuarioId);
    }

    // Actualizar cantidad de un producto
    public CarritoRespuestaDto? ActualizarItem(
        int usuarioId,
        int itemId,
        ActualizarItemCarritoDto item)
    {
        if (item.Cantidad <= 0)
        {
            throw new ArgumentException(
                "La cantidad debe ser mayor a cero."
            );
        }

        var carrito = _carritoRepository.ObtenerPorUsuario(usuarioId);

        if (carrito == null)
        {
            return null;
        }

        var detalle = _detalleService.ObtenerPorId(
            itemId,
            carrito.Id
        );

        if (detalle == null)
        {
            return null;
        }

        var producto = _productoRepository.ObtenerPorId(
            detalle.ProductoId
        );

        if (producto == null || !producto.Activo || producto.Stock <= 0)
        {
            throw new InvalidOperationException(
                "El producto no está disponible."
            );
        }

        if (item.Cantidad > producto.Stock)
        {
            throw new InvalidOperationException(
                "La cantidad solicitada supera el stock disponible."
            );
        }

        _detalleService.ActualizarCantidad(
            detalle.Id,
            carrito.Id,
            item.Cantidad
        );

        _carritoRepository.ActualizarFecha(
            carrito.Id,
            DateTime.UtcNow
        );

        return ObtenerCarrito(usuarioId);
    }

    // Eliminar producto del carrito
    public bool EliminarItem(int usuarioId, int itemId)
    {
        var carrito = _carritoRepository.ObtenerPorUsuario(usuarioId);

        if (carrito == null)
        {
            return false;
        }

        bool eliminado = _detalleService.Eliminar(itemId, carrito.Id);

        if (!eliminado)
        {
            return false;
        }

        _carritoRepository.ActualizarFecha(
            carrito.Id,
            DateTime.UtcNow
        );

        return true;
    }

    // Obtener el carrito existente o crear uno nuevo
    private Carrito ObtenerOCrearCarrito(int usuarioId)
    {
        var carrito = _carritoRepository.ObtenerPorUsuario(usuarioId);

        if (carrito != null)
        {
            return carrito;
        }

        var nuevoCarrito = new Carrito
        {
            UsuarioId = usuarioId,
            FechaActualizacion = DateTime.UtcNow
        };

        _carritoRepository.Agregar(nuevoCarrito);

        return _carritoRepository.ObtenerPorUsuario(usuarioId)
            ?? throw new InvalidOperationException(
                "No fue posible crear el carrito."
            );
    }

    // Construir la respuesta con los importes calculados
    private CarritoRespuestaDto CrearRespuesta(Carrito carrito)
    {
        var items = carrito.DetallesCarrito.Select(detalle =>
        {
            var producto = detalle.Producto
                ?? throw new InvalidOperationException(
                    "No se encontró un producto relacionado con el carrito."
                );

            decimal subtotal = producto.Precio * detalle.Cantidad;

            return new DetalleCarritoRespuestaDto
            {
                Id = detalle.Id,
                ProductoId = producto.Id,
                NombreProducto = producto.Nombre,
                ImagenUrl = producto.ImagenUrl,
                PrecioUnitario = producto.Precio,
                Cantidad = detalle.Cantidad,
                Subtotal = subtotal,
                StockDisponible = producto.Stock,
                ProductoActivo = producto.Activo
            };
        }).ToList();

        decimal subtotalCarrito = items.Sum(i => i.Subtotal);

        decimal costoEnvio = subtotalCarrito == 0
            ? 0
            : subtotalCarrito < 999
                ? 99
                : 0;

        return new CarritoRespuestaDto
        {
            Id = carrito.Id,
            FechaActualizacion = carrito.FechaActualizacion,
            Items = items,
            Subtotal = subtotalCarrito,
            CostoEnvio = costoEnvio,
            Total = subtotalCarrito + costoEnvio
        };
    }
}
