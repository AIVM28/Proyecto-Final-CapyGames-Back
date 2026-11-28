using Microsoft.AspNetCore.Hosting;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public class CarritoJsonRepository : JsonRepositoryBase<Carrito>, ICarritoRepository
{
    public CarritoJsonRepository(IWebHostEnvironment environment)
        : base(
            environment,
            "carritos.json",
            carrito => new Carrito
            {
                Id = carrito.Id,
                UsuarioId = carrito.UsuarioId,
                FechaActualizacion = carrito.FechaActualizacion,
                Usuario = null,
                DetallesCarrito = new List<DetalleCarrito>()
            },
            carrito => carrito.Id,
            (carrito, id) => carrito.Id = id)
    {
    }

    public Carrito? ObtenerPorUsuario(int usuarioId)
    {
        return BuscarBase(carrito => carrito.UsuarioId == usuarioId);
    }

    public Carrito Agregar(Carrito nuevoCarrito)
    {
        nuevoCarrito.Usuario = null;
        nuevoCarrito.DetallesCarrito = new List<DetalleCarrito>();
        return AgregarBase(nuevoCarrito);
    }

    public bool ActualizarFecha(int id, DateTime fechaActualizacion)
    {
        return ActualizarBase(id, carrito =>
        {
            carrito.FechaActualizacion = fechaActualizacion;
        });
    }
}
