
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public interface ICarritoRepository
{
    Carrito? ObtenerPorUsuario(int usuarioId);

    Carrito Agregar(Carrito nuevoCarrito);

    bool ActualizarFecha(int id, DateTime fechaActualizacion);
}
