using Presentation.Endpoints;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios de Infrastructure y Application (DbContext, repos y MediatR)
builder.Services.AddInfrastructure(builder.Configuration);

// Agregar servicios al contenedor.
// Más información sobre la configuración de OpenAPI: https://aka.ms/aspnet/openapi
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
