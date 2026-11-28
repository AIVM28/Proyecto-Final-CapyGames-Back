using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Proyecto_Final_CapyGames_Back.Data;
using Proyecto_Final_CapyGames_Back.Repositories;
using Proyecto_Final_CapyGames_Back.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

//PosgreSQL
// var connectionString = builder.Configuration
//     .GetConnectionString("PostgreSQLConnection");

// if (string.IsNullOrWhiteSpace(connectionString))
// {
//     throw new InvalidOperationException(
//         "No se encontró la cadena de conexión PostgreSQLConnection."
//     );
// }


//JSON
builder.Services.AddScoped<IUsuarioRepository, UsuarioJsonRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaJsonRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoJsonRepository>();
builder.Services.AddScoped<IDireccionRepository, DireccionJsonRepository>();
builder.Services.AddScoped<ICarritoRepository, CarritoJsonRepository>();
builder.Services.AddScoped<IDetalleCarritoRepository, DetalleCarritoJsonRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoJsonRepository>();
builder.Services.AddScoped<IDetallePedidoRepository, DetallePedidoJsonRepository>();

//PosgreSQL
// builder.Services.AddDbContext<PostgreSqlDbContext>(options =>
//     options.UseNpgsql(connectionString)
// );


//PosgreSQL
// builder.Services.AddScoped<
//     IUsuarioRepository,
//     UsuarioPostgreSqlRepository
// >();

// Servicios de la aplicación
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICarritoService, CarritoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IDetalleCarritoService, DetalleCarritoService>();
builder.Services.AddScoped<IDetallePedidoService, DetallePedidoService>();
builder.Services.AddScoped<IDireccionService, DireccionService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();


var jwt = builder.Configuration.GetSection("Jwt");

var keyBase64 = jwt["Key"]
    ?? throw new InvalidOperationException(
        "Falta configurar Jwt:Key."
    );

byte[] key;

try
{
    key = Convert.FromBase64String(keyBase64);
}
catch (FormatException)
{
    throw new InvalidOperationException(
        "Jwt:Key no contiene una clave válida en Base64."
    );
}

if (key.Length < 32)
{
    throw new InvalidOperationException(
        "La clave JWT debe contener al menos 32 bytes."
    );
}

var issuer = jwt["Issuer"]
    ?? throw new InvalidOperationException(
        "Falta configurar Jwt:Issuer."
    );

var audience = jwt["Audience"]
    ?? throw new InvalidOperationException(
        "Falta configurar Jwt:Audience."
    );

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = true;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                ValidateLifetime = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,

                ValidAlgorithms = new[]
                {
                    SecurityAlgorithms.HmacSha256
                },

                ClockSkew = TimeSpan.Zero
            };
    });



builder.Services.AddAuthorization();



var app = builder.Build();

// Habilitar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
