using Presentation.Endpoints;
using Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios de Infrastructure y Application (DbContext, repos y MediatR)
builder.Services.AddInfrastructure(builder.Configuration);

// Agregar servicios al contenedor.
// Más información sobre la configuración de OpenAPI: https://aka.ms/aspnet/openapi
// Configurar JSON para que no escriba propiedades con valor null
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(opts =>
{
    opts.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddOpenApi();

var app = builder.Build();

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
app.MapUserEndpoints();

app.Run();
