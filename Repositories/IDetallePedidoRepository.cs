using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public interface IDetallePedidoRepository
{
    List<DetallePedido> ObtenerTodos();
    DetallePedido? ObtenerPorId(int id);
    DetallePedido Agregar(DetallePedido nuevoDetallePedido);
    bool Actualizar(int id, DetallePedido detallePedido);
    bool Eliminar(int id);
}