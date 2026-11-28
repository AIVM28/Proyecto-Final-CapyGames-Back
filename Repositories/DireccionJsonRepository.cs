using Microsoft.AspNetCore.Hosting;
using Proyecto_Final_CapyGames_Back.Models;

namespace Proyecto_Final_CapyGames_Back.Repositories;

public class DireccionJsonRepository : JsonRepositoryBase<Direccion>, IDireccionRepository
{
    public DireccionJsonRepository(IWebHostEnvironment environment)
        : base(
            environment,
            "direcciones.json",
            direccion => new Direccion
            {
                Id = direccion.Id,
                UsuarioId = direccion.UsuarioId,
                Calle = direccion.Calle,
                NumeroExterior = direccion.NumeroExterior,
                NumeroInterior = direccion.NumeroInterior,
                Colonia = direccion.Colonia,
                Ciudad = direccion.Ciudad,
                Estado = direccion.Estado,
                CodigoPostal = direccion.CodigoPostal,
                Referencias = direccion.Referencias,
                Predeterminada = direccion.Predeterminada,
                Usuario = null
            },
            direccion => direccion.Id,
            (direccion, id) => direccion.Id = id)
    {
    }

    public List<Direccion> ObtenerPorUsuario(int usuarioId)
    {
        return ObtenerTodosBase()
            .Where(direccion => direccion.UsuarioId == usuarioId)
            .ToList();
    }

    public Direccion? ObtenerPorId(int id, int usuarioId)
    {
        return BuscarBase(direccion =>
            direccion.Id == id &&
            direccion.UsuarioId == usuarioId);
    }

    public Direccion Agregar(Direccion nuevaDireccion)
    {
        nuevaDireccion.Usuario = null;
        return AgregarBase(nuevaDireccion);
    }

    public bool Actualizar(int id, int usuarioId, Direccion direccion)
    {
        return ActualizarBase(
            id,
            existente => existente.UsuarioId == usuarioId,
            existente =>
            {
                existente.Calle = direccion.Calle;
                existente.NumeroExterior = direccion.NumeroExterior;
                existente.NumeroInterior = direccion.NumeroInterior;
                existente.Colonia = direccion.Colonia;
                existente.Ciudad = direccion.Ciudad;
                existente.Estado = direccion.Estado;
                existente.CodigoPostal = direccion.CodigoPostal;
                existente.Referencias = direccion.Referencias;
                existente.Predeterminada = direccion.Predeterminada;
            });
    }

    public bool Eliminar(int id, int usuarioId)
    {
        return EliminarBase(
            id,
            direccion => direccion.UsuarioId == usuarioId);
    }
}
