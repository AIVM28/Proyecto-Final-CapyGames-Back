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

    public List<Carrito> ObtenerTodos() => ObtenerTodosBase();

    public Carrito? ObtenerPorId(int id) => ObtenerPorIdBase(id);

    public Carrito Agregar(Carrito nuevoCarrito) => AgregarBase(nuevoCarrito);

    public bool Actualizar(int id, Carrito carrito)
    {
        return ActualizarBase(id, existente =>
        {
            existente.UsuarioId = carrito.UsuarioId;
            existente.FechaActualizacion = carrito.FechaActualizacion;
        });
    }

    public bool Eliminar(int id) => EliminarBase(id);
}
