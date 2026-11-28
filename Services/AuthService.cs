
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using Proyecto_Final_CapyGames_Back.DTOs;
using Proyecto_Final_CapyGames_Back.Models;
using Proyecto_Final_CapyGames_Back.Repositories;

namespace Proyecto_Final_CapyGames_Back.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<Usuario> _passwordHasher;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<Usuario>();
    }

    public UsuarioRespuestaDto Registrar(RegistroDto registro)
    {
        string email = registro.Email.Trim().ToLowerInvariant();

        var existente = _usuarioRepository.ObtenerPorEmail(email);

        if (existente != null)
        {
            throw new InvalidOperationException(
                "El correo electrónico ya está registrado.");
        }

        var usuario = new Usuario
        {
            Nombre = registro.Nombre.Trim(),
            Apellidos = registro.Apellidos.Trim(),
            Email = email,
            Telefono = registro.Telefono.Trim(),
            Rol = "Cliente",
            FechaRegistro = DateTime.UtcNow,
            Activo = true
        };

        usuario.PasswordHash = _passwordHasher.HashPassword(
            usuario,
            registro.Password
        );

        var usuarioGuardado = _usuarioRepository.Agregar(usuario);

        return CrearUsuarioRespuesta(usuarioGuardado);
    }

    // ==========================================
    // INICIO DE SESION
    // ==========================================

    public AuthRespuestaDto? IniciarSesion(LoginDto login)
    {
        string email = login.Email.Trim().ToLowerInvariant();

        var usuario = _usuarioRepository.ObtenerPorEmail(email);

        if (usuario == null || !usuario.Activo)
        {
            return null;
        }

        var resultado = _passwordHasher.VerifyHashedPassword(
            usuario,
            usuario.PasswordHash,
            login.Password
        );

        if (resultado == PasswordVerificationResult.Failed)
        {
            return null;
        }

        // Actualizar el hash si el algoritmo recomienda renovarlo.
        if (resultado == PasswordVerificationResult.SuccessRehashNeeded)
        {
            usuario.PasswordHash = _passwordHasher.HashPassword(
                usuario,
                login.Password
            );

            _usuarioRepository.Actualizar(usuario.Id, usuario);
        }

        string token = GenerarToken(usuario);

        return new AuthRespuestaDto
        {
            Token = token,
            Usuario = CrearUsuarioRespuesta(usuario)
        };
    }

    // ==========================================
    // CONSULTAR PERFIL
    // ==========================================

    public UsuarioRespuestaDto? ObtenerPerfil(int usuarioId)
    {
        var usuario = _usuarioRepository.ObtenerPorId(usuarioId);

        if (usuario == null || !usuario.Activo)
        {
            return null;
        }

        return CrearUsuarioRespuesta(usuario);
    }

    // ==========================================
    // ACTUALIZAR PERFIL
    // ==========================================

    public UsuarioRespuestaDto? ActualizarPerfil(
        int usuarioId,
        ActualizarPerfilDto perfil)
    {
        var usuario = _usuarioRepository.ObtenerPorId(usuarioId);

        if (usuario == null || !usuario.Activo)
        {
            return null;
        }

        string email = perfil.Email.Trim().ToLowerInvariant();

        var usuarioConEmail = _usuarioRepository.ObtenerPorEmail(email);

        if (usuarioConEmail != null && usuarioConEmail.Id != usuarioId)
        {
            throw new InvalidOperationException(
                "El correo electrónico ya está registrado por otro usuario.");
        }

        usuario.Nombre = perfil.Nombre.Trim();
        usuario.Apellidos = perfil.Apellidos.Trim();
        usuario.Email = email;
        usuario.Telefono = perfil.Telefono.Trim();

        bool actualizado = _usuarioRepository.Actualizar(
            usuarioId,
            usuario
        );

        if (!actualizado)
        {
            return null;
        }

        return CrearUsuarioRespuesta(usuario);
    }

    // ==========================================
    // GENERAR JWT
    // ==========================================

    private string GenerarToken(Usuario usuario)
    {
        var jwt = _configuration.GetSection("Jwt");

        string claveBase64 = jwt["Key"]
            ?? throw new InvalidOperationException(
                "No se configuró Jwt:Key.");

        byte[] clave;

        try
        {
            clave = Convert.FromBase64String(claveBase64);
        }
        catch (FormatException ex)
        {
            throw new InvalidOperationException(
                "Jwt:Key debe contener una clave válida en Base64.",
                ex
            );
        }

        if (clave.Length < 32)
        {
            throw new InvalidOperationException(
                "La clave JWT debe contener al menos 32 bytes."
            );
        }

        string issuer = jwt["Issuer"]
            ?? throw new InvalidOperationException(
                "No se configuró Jwt:Issuer.");

        string audience = jwt["Audience"]
            ?? throw new InvalidOperationException(
                "No se configuró Jwt:Audience.");

        int.TryParse(
            jwt["ExpirationMinutes"],
            out int expirationMinutes
        );

        if (expirationMinutes <= 0)
        {
            expirationMinutes = 30;
        }

        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                usuario.Id.ToString()
            ),

            new Claim(
                ClaimTypes.Name,
                usuario.Email
            ),

            new Claim(
                ClaimTypes.Email,
                usuario.Email
            ),

            new Claim(
                ClaimTypes.Role,
                usuario.Rol
            )
        };

        var signingKey = new SymmetricSecurityKey(clave);

        var credentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // ==========================================
    // CONVERTIR ENTIDAD A DTO DE RESPUESTA
    // ==========================================

    private UsuarioRespuestaDto CrearUsuarioRespuesta(Usuario usuario)
    {
        return new UsuarioRespuestaDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellidos = usuario.Apellidos,
            Email = usuario.Email,
            Telefono = usuario.Telefono,
            Rol = usuario.Rol,
            FechaRegistro = usuario.FechaRegistro
        };
    }
}
