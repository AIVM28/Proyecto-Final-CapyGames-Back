using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;

namespace Proyecto_Final_CapyGames_Back.Repositories;

/// <summary>
/// Implementa las operaciones comunes de persistencia en archivos JSON.
/// Las clases concretas indican cómo obtener/asignar el Id y cómo copiar
/// solamente los campos escalares que deben persistirse.
/// </summary>
public abstract class JsonRepositoryBase<T> where T : class
{
    private static readonly ConcurrentDictionary<string, object> FileLocks =
        new(StringComparer.OrdinalIgnoreCase);

    private readonly string _filePath;
    private readonly object _fileLock;
    private readonly Func<T, T> _prepareForStorage;
    private readonly Func<T, int> _getId;
    private readonly Action<T, int> _setId;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    protected JsonRepositoryBase(
        IWebHostEnvironment environment,
        string fileName,
        Func<T, T> prepareForStorage,
        Func<T, int> getId,
        Action<T, int> setId)
    {
        ArgumentNullException.ThrowIfNull(environment);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        _prepareForStorage = prepareForStorage
            ?? throw new ArgumentNullException(nameof(prepareForStorage));
        _getId = getId ?? throw new ArgumentNullException(nameof(getId));
        _setId = setId ?? throw new ArgumentNullException(nameof(setId));

        var dataDirectory = Path.Combine(
            environment.ContentRootPath,
            "Data",
            "JsonData");

        Directory.CreateDirectory(dataDirectory);

        _filePath = Path.GetFullPath(Path.Combine(dataDirectory, fileName));
        _fileLock = FileLocks.GetOrAdd(_filePath, static _ => new object());
    }

    protected List<T> ObtenerTodosBase()
    {
        lock (_fileLock)
        {
            return LeerTodos();
        }
    }

    protected T? ObtenerPorIdBase(int id)
    {
        lock (_fileLock)
        {
            return LeerTodos().FirstOrDefault(item => _getId(item) == id);
        }
    }

    protected T? BuscarBase(Func<T, bool> criterio)
    {
        ArgumentNullException.ThrowIfNull(criterio);

        lock (_fileLock)
        {
            return LeerTodos().FirstOrDefault(criterio);
        }
    }

    protected T AgregarBase(T entidad)
    {
        ArgumentNullException.ThrowIfNull(entidad);

        lock (_fileLock)
        {
            var elementos = LeerTodos();
            var siguienteId = elementos.Count == 0
                ? 1
                : checked(elementos.Max(_getId) + 1);

            // Igual que las implementaciones PostgreSQL, el Id lo genera el repositorio.
            _setId(entidad, siguienteId);
            elementos.Add(entidad);
            GuardarTodos(elementos);

            return entidad;
        }
    }

    protected bool ActualizarBase(int id, Action<T> aplicarCambios)
    {
        ArgumentNullException.ThrowIfNull(aplicarCambios);

        lock (_fileLock)
        {
            var elementos = LeerTodos();
            var existente = elementos.FirstOrDefault(item => _getId(item) == id);

            if (existente is null)
            {
                return false;
            }

            aplicarCambios(existente);
            GuardarTodos(elementos);
            return true;
        }
    }

    protected bool EliminarBase(int id)
    {
        lock (_fileLock)
        {
            var elementos = LeerTodos();
            var existente = elementos.FirstOrDefault(item => _getId(item) == id);

            if (existente is null)
            {
                return false;
            }

            elementos.Remove(existente);
            GuardarTodos(elementos);
            return true;
        }
    }

    private List<T> LeerTodos()
    {
        if (!File.Exists(_filePath))
        {
            return new List<T>();
        }

        var json = File.ReadAllText(_filePath, Encoding.UTF8);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<T>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions)
                ?? new List<T>();
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException(
                $"El archivo '{Path.GetFileName(_filePath)}' contiene JSON inválido. " +
                "No se sobrescribieron sus datos.",
                exception);
        }
    }

    private void GuardarTodos(List<T> elementos)
    {
        // Se preparan copias sin propiedades de navegación para evitar ciclos
        // y mantener cada entidad en su propio archivo JSON.
        var datos = elementos.Select(_prepareForStorage).ToList();
        var json = JsonSerializer.Serialize(datos, _jsonOptions);

        // Escritura temporal y reemplazo para reducir el riesgo de dejar
        // un archivo incompleto si la aplicación se interrumpe al escribir.
        var temporaryPath = _filePath + "." + Guid.NewGuid().ToString("N") + ".tmp";

        try
        {
            File.WriteAllText(temporaryPath, json, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            File.Move(temporaryPath, _filePath, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }
}
