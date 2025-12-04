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
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

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

app.Run();
