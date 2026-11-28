
using Proyecto_Final_CapyGames_Back.DTOs;

namespace Proyecto_Final_CapyGames_Back.Services;

public interface IPedidoService
{
    PedidoRespuestaDto CrearPedido(int usuarioId, CrearPedidoDto pedido);

    List<PedidoRespuestaDto> ObtenerPorUsuario(int usuarioId);

    PedidoRespuestaDto? ObtenerDetalle(int id, int usuarioId);

    bool Cancelar(int id, int usuarioId);

    List<PedidoRespuestaDto> ObtenerTodosAdmin(string? estado);

    bool ActualizarEstadoAdmin(int id, string estado);

    ResumenAdminDto ObtenerResumenAdmin();
}
