using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public interface IPedidoRepository
{
    List<Pedido> ObtenerTodos();
    Pedido? ObtenerPorId(int id);
    Pedido Agregar(Pedido nuevoPedido);
    bool Actualizar(int id, Pedido pedido);
    bool Eliminar(int id);
}