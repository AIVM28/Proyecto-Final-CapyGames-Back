using Proyecto_Final_CapyGames_Back.Models;
using Proyecto_Final_CapyGames_Back.Repositories;

namespace Proyecto_Final_CapyGames_Back.Services;

public class DireccionService : IDireccionService
{
    private readonly IDireccionRepository _repository;

    public DireccionService(IDireccionRepository repository)
    {
        _repository = repository;
    }

    // Consultar direcciones del usuario
    public List<Direccion> ObtenerPorUsuario(int usuarioId)
    {
        return _repository.ObtenerPorUsuario(usuarioId);
    }

    // Agregar dirección
    public Direccion Agregar(int usuarioId, Direccion nuevaDireccion)
    {
        ValidarDireccion(nuevaDireccion);

        nuevaDireccion.Id = 0;
        nuevaDireccion.UsuarioId = usuarioId;
        nuevaDireccion.Usuario = null;

        if (nuevaDireccion.Predeterminada)
        {
            DesmarcarPredeterminadas(usuarioId);
        }

        return _repository.Agregar(nuevaDireccion);
    }

    // Actualizar dirección
    public bool Actualizar(int id, int usuarioId, Direccion direccion)
    {
        var existente = _repository.ObtenerPorId(id, usuarioId);

        if (existente == null)
        {
            return false;
        }

        ValidarDireccion(direccion);

        if (direccion.Predeterminada)
        {
            DesmarcarPredeterminadas(usuarioId, id);
        }

        existente.Calle = direccion.Calle;
        existente.NumeroExterior = direccion.NumeroExterior;
        existente.NumeroInterior = direccion.NumeroInterior;
        existente.Colonia = direccion.Colonia;
        existente.Ciudad = direccion.Ciudad;
        existente.Estado = direccion.Estado;
        existente.CodigoPostal = direccion.CodigoPostal;
        existente.Referencias = direccion.Referencias;
        existente.Predeterminada = direccion.Predeterminada;

        return _repository.Actualizar(id, usuarioId, existente);
    }

    // Eliminar dirección
    public bool Eliminar(int id, int usuarioId)
    {
        var existente = _repository.ObtenerPorId(id, usuarioId);

        if (existente == null)
        {
            return false;
        }

        return _repository.Eliminar(id, usuarioId);
    }

    // Validar información de dirección
    private void ValidarDireccion(Direccion direccion)
    {
        if (string.IsNullOrWhiteSpace(direccion.Calle))
            throw new ArgumentException("La calle es obligatoria.");

        if (string.IsNullOrWhiteSpace(direccion.NumeroExterior))
            throw new ArgumentException("El número exterior es obligatorio.");

        if (string.IsNullOrWhiteSpace(direccion.Colonia))
            throw new ArgumentException("La colonia es obligatoria.");

        if (string.IsNullOrWhiteSpace(direccion.Ciudad))
            throw new ArgumentException("La ciudad es obligatoria.");

        if (string.IsNullOrWhiteSpace(direccion.Estado))
            throw new ArgumentException("El estado es obligatorio.");

        if (string.IsNullOrWhiteSpace(direccion.CodigoPostal))
            throw new ArgumentException("El código postal es obligatorio.");
    }

    // Garantizar una sola dirección predeterminada
    private void DesmarcarPredeterminadas(
        int usuarioId,
        int? direccionExcluir = null)
    {
        var direcciones = _repository.ObtenerPorUsuario(usuarioId);

        foreach (var direccion in direcciones)
        {
            if (direccion.Predeterminada &&
                direccion.Id != direccionExcluir)
            {
                direccion.Predeterminada = false;

                _repository.Actualizar(
                    direccion.Id,
                    usuarioId,
                    direccion
                );
            }
        }
    }
}
