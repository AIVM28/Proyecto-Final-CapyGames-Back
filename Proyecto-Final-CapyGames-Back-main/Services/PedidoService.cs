using System.Reflection.Metadata.Ecma335;
using Proyecto_Final_CapyGames_Back.Models;
using Proyecto_Final_CapyGames_Back.Repositories;

namespace Proyecto_Final_CapyGames_Back.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _repository;

    public PedidoService(IPedidoRepository repository)
    {
        _repository = repository;
    }

    public List<Pedido> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public Pedido? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public Pedido Agregar(Pedido nuevoPedido)
    {

        return _repository.Agregar(nuevoPedido);
    }

    public bool Actualizar(int id, Pedido pedido)
    {

        return _repository.Actualizar(id, pedido);
    }

    public bool Eliminar(int id)
    {
        var existente = _repository.ObtenerPorId(id);
        if (existente == null)
        {
            return false;
        }

        return _repository.Eliminar(id);
    }

    private decimal costoPedido(Pedido pedido)
    {
        if (pedido.ProductoCosto > 999)
        {
            return 99;
        } 
        return 0;       
    }

}