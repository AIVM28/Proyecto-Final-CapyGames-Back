using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public interface IPedidoRepository
{
    List<Pedido> ObtenerPorUsuario(int usuarioId);

    Pedido? ObtenerPorIdUsuario(int id, int usuarioId);

    List<Pedido> ObtenerTodos(string? estado = null);

    Pedido? ObtenerPorId(int id);

    Pedido Agregar(Pedido nuevoPedido);

    void GuardarCambios();
}
