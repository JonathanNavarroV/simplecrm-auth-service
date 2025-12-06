using Presentation.Endpoints;
using Infrastructure;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;

// Program.cs - punto de entrada del servicio Auth
// Este archivo registra los servicios necesarios y configura el pipeline HTTP.

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------
// 1) Infraestructura y servicios de la aplicación
// -------------------------------------------------
// `AddInfrastructure` registra el DbContext, repositorios, y (si aplica) los
// servicios de la capa Application (por ejemplo MediatR handlers).
// Revisa `Infrastructure/DependencyInjection.cs` para más detalles.
builder.Services.AddInfrastructure(builder.Configuration);

// -------------------------------------------------
// 2) Configuración JSON
// -------------------------------------------------
// Evita serializar propiedades con valor null en las respuestas JSON.
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(opts =>
{
    opts.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// -------------------------------------------------
// 3) OpenAPI / documentación
// -------------------------------------------------
// Habilita generación de OpenAPI (Swagger) para documentación automática.
builder.Services.AddOpenApi();

// -------------------------------------------------
// 4) Autenticación JWT (opcional según configuración)
// -------------------------------------------------
// Si en configuración se encuentra Jwt:SigningKeyBase64, registramos un esquema
// JWT con clave simétrica para validar tokens internos emitidos por JwtTokenFactory.
// Alternativamente, en entornos productivos puede usarse Entra ID / OpenID Connect.
var signingKeyBase64 = builder.Configuration["Jwt:SigningKeyBase64"];
var _authConfigured = false;
if (!string.IsNullOrEmpty(signingKeyBase64))
{
    var key = new SymmetricSecurityKey(Convert.FromBase64String(signingKeyBase64));
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // Comprueba emisor/audiencia/firmas y caducidad del token
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = true
            };
        });
    builder.Services.AddAuthorization();
    _authConfigured = true;
}

// -------------------------------------------------
// 5) Construir la aplicación y configurar pipeline
// -------------------------------------------------
var app = builder.Build();

// Si se configuró autenticación, conectar los middlewares correspondientes.
if (_authConfigured)
{
    // `UseAuthentication` valida la identidad; `UseAuthorization` aplica políticas.
    app.UseAuthentication();
    app.UseAuthorization();
}

// En desarrollo exponemos la UI de OpenAPI.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// En entornos no desarrollos forzamos redirección a HTTPS (si está configurado).
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// -------------------------------------------------
// 6) Endpoints públicos
// -------------------------------------------------
// Healthcheck sencillo para que el gateway (YARP) y orquestadores verifiquen
// que el servicio está arriba. Devuelve estado y timestamp UTC.
app.MapGet("/healthz", () => Results.Ok(new { status = "healthy", time = DateTime.UtcNow }))
    .WithName("HealthCheck");

// Mapear conjuntos de endpoints definidos en `Presentation/Endpoints/*`.
// - `MapUserEndpoints` agrupa rutas relacionadas con user management.
// - `MapAuthEndpoints` agrupa rutas de intercambio de tokens / autenticación.
app.MapUserEndpoints();
app.MapAuthEndpoints();

// Arranca el servidor Kestrel con la configuración por defecto (puertos en appsettings o variables de entorno)
app.Run();
