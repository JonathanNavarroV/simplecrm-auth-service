using Presentation.Endpoints;
using Infrastructure;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;

// Archivo limpio de Program.cs — configuración mínima y estable para el Auth Service.
var builder = WebApplication.CreateBuilder(args);

// Registrar servicios de Infrastructure y Application (DbContext, repos y MediatR).
builder.Services.AddInfrastructure(builder.Configuration);

// Configuración JSON: omitir propiedades null en las respuestas
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(opts =>
{
    opts.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// OpenAPI
builder.Services.AddOpenApi();

// Autenticación basada en clave simétrica (opcional, si está configurada en appsettings)
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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapGet("/healthz", () => Results.Ok(new { status = "healthy", time = DateTime.UtcNow }))
    .WithName("HealthCheck");

app.MapUserEndpoints();
app.MapAuthEndpoints();

app.Run();
