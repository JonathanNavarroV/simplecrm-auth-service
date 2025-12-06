<<<<<<< Updated upstream
using Presentation.Endpoints;
using Infrastructure;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
// Identity/OpenID usings are registered where needed in services/endpoints

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios de Infrastructure y Application (DbContext, repos y MediatR).
// Nota: `AddInfrastructure` también invoca `AddApplication` internamente para
// registrar los handlers de MediatR y otros servicios de la capa Application.
builder.Services.AddInfrastructure(builder.Configuration);

// Agregar servicios al contenedor (OpenAPI y configuración JSON).
// Más información sobre la configuración de OpenAPI: https://aka.ms/aspnet/openapi
// Configurar JSON para que no escriba propiedades con valor null
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(opts =>
{
    opts.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddOpenApi();

// Registrar autenticación/autorization para validar tokens internos emitidos por JwtTokenFactory
// Este middleware valida los JWT internos que el gateway inyectará en Authorization.
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

var app = builder.Build();

if (_authConfigured)
{
    app.UseAuthentication();
    app.UseAuthorization();
}

// Configurar el pipeline de solicitudes HTTP.
=======
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

// Se crea el "builder" de la aplicación
var builder = WebApplication.CreateBuilder(args);

#region --- Servicios base (Controllers, Swagger, JSON, HttpContext) ---

// Controllers: Habilita el modelo MVC para exponer endpoints con [ApiController]
builder.Services.AddControllers();

// OpenAPI + Scalar: Documenta la API y ofrece una UI moderna
// - AddOpenApi registra el generador de especificación (documento OpenAPI).
// - Scalar se mapea más abajo en el pipeline
builder.Services.AddOpenApi();

// Opciones JSON: Respeta los nombres originales y omite valores null en las respuestas
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = System
        .Text
        .Json
        .Serialization
        .JsonIgnoreCondition
        .WhenWritingNull; // No escribe nulls
});

// HttpContextAccessor: Permite leer HttpContext (claims del usuario, headers, etc)
builder.Services.AddHttpContextAccessor();

#endregion

#region --- Infraestructura y Aplicación ---

// Infraestructura: Registra EF Core, DbContext, repositorios, etc.
// Usa la ConnecionString "AuthDb" de appsettings.* (o variables de entorno)
builder.Services.AddInfrastructure(builder.Configuration);

// builder.Services.AddApplication();

#endregion

#region --- Autenticación y Autorización (Entra ID como esquema principal) ---

// Autenticación JWT con Entra ID
// - Authority: URL del tenant para validar la firma de tokens.
// - ValidIssuers / ValidAudiences: Listas permitidas (Cargadas desde appsettings.*)
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // "Bearer"
    .AddJwtBearer(options =>
    {
        // En desarrollo puede no ser HTTPS
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();

        // Authority de Entra ID
        options.Authority = builder.Configuration["Authentication:EntraId:Authority"];

        // Reglas de validacióndel token
        options.TokenValidationParameters = new TokenValidationParameters { ValidateIssuer = true };
    });

// Autorización Politicas/Roles
builder.Services.AddAuthorization();

#endregion

#region --- HealthChecks ---

// Permite exponer checks de salud. Si AddInfrastructure registra el DbContext se podrán añadir checks de DB ahí.
builder.Services.AddHealthChecks();

#endregion

// Se contruye la app con los servicios registrados
var app = builder.Build();

#region --- Middleware y pipeline HTTP ---

// Se publica el documento OpenAPI + UI de Scalar solo en Dev
>>>>>>> Stashed changes
if (app.Environment.IsDevelopment())
{
    // Publica el documento en /openapi/v1.json
    app.MapOpenApi();

    // Mapea la UI de Scalar
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("SimpleCRM Auth Service API") // Título
            .WithDefaultOpenAllTags(); // Expande secciones por defecto;
    });
}

<<<<<<< Updated upstream
// Evitar la redirección HTTPS en entorno de desarrollo para no mostrar
// el warning: "Failed to determine the https port for redirect.".
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Endpoint de salud simple para monitoreo por el gateway (YARP)
app.MapGet("/healthz", () => Results.Ok(new { status = "healthy", time = DateTime.UtcNow }))
    .WithName("HealthCheck");

// Registrar endpoints agrupados
// Centraliza las rutas relacionadas con usuarios (GET/POST/PUT/etc.).
// Ver `Presentation/Endpoints/UserEndpoints.cs` para la definición.
app.MapUserEndpoints();
// Registrar endpoints de Auth (exchange)
app.MapAuthEndpoints();
=======
// Autenticación y Autorización deben ir antes de de mapear endpoints que las requieran
app.UseAuthentication();
app.UseAuthorization();

#endregion

#region --- Endpoints ---
>>>>>>> Stashed changes

// Mapea los controladores (Presentation/Controllers/*)
app.MapControllers();

// Health
app.MapHealthChecks("/healthz");

#endregion

// Arranca el servidor Kestrel y queda escuchando en los puertos configurados
app.Run();
