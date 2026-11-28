using Proyecto_Final_CapyGames_Back.Models;
using Proyecto_Final_CapyGames_Back.Repositories;

namespace Proyecto_Final_CapyGames_Back.Services;

public class DetallePedidoService : IDetallePedidoService
{
    private readonly IDetallePedidoRepository _repository;

    public DetallePedidoService(IDetallePedidoRepository repository)
    {
        _repository = repository;
    }

    public List<DetallePedido> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public DetallePedido? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public DetallePedido Agregar(DetallePedido nuevoDetallePedido)
    {


        return _repository.Agregar(nuevoDetallePedido);
    }

    public bool Actualizar(int id, DetallePedido DetallePedido)
    {

        return _repository.Actualizar(id, DetallePedido);
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

}