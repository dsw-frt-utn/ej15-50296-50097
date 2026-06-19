using Dsw2026Ej15.Api.Exceptions;
using Dsw2026Ej15.Api.Midleware;
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain;
using Dsw2026Ej15.Domain.Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// 1. REGISTRAR LA PERSISTENCIA EN EL CONTENEDOR DE DEPENDENCIAS
// Aquí le indicas a .NET: "Cada vez que un componente pida IPersistence, entrégale la instancia única de PersistenceInMemory"
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<IPersistence, PersistenceInMemory>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//PersistenceTest.TestPersistenceAsync(app).Wait();
// Configuración normal del pipeline de la API
if (app.Environment.IsDevelopment())
{ 
    // Habilitar Swagger en desarrollo y abrir la UI de Swagger en la raíz
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();

// Basic health-check endpoint
app.MapGet("/health-check", () => Results.Ok(new { status = "Healthy" }));

app.MapControllers();

app.Run();
