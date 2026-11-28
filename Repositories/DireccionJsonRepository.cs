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

    public List<Direccion> ObtenerTodas() => ObtenerTodosBase();

    public Direccion? ObtenerPorId(int id) => ObtenerPorIdBase(id);

    public Direccion Agregar(Direccion nuevaDireccion) => AgregarBase(nuevaDireccion);

    public bool Actualizar(int id, Direccion direccion)
    {
        return ActualizarBase(id, existente =>
        {
            existente.UsuarioId = direccion.UsuarioId;
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

    public bool Eliminar(int id) => EliminarBase(id);
}
