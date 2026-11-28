
using Proyecto_Final_CapyGames_Back.DTOs;

namespace Proyecto_Final_CapyGames_Back.Services;

public interface ICarritoService
{
    CarritoRespuestaDto ObtenerCarrito(int usuarioId);

    CarritoRespuestaDto AgregarItem(
        int usuarioId,
        AgregarItemCarritoDto item
    );

    CarritoRespuestaDto? ActualizarItem(
        int usuarioId,
        int itemId,
        ActualizarItemCarritoDto item
    );

    bool EliminarItem(int usuarioId, int itemId);
}
