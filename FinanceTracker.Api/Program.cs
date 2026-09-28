using FinanceTracker.Api.Endpoints;
using FinanceTracker.Api.Middleware;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Data;
using FinanceTracker.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Inyección de dependencias
builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<ICuentaService, CuentaService>();

builder.Services.AddScoped<ICategoriaService, CategoriaService>();

builder.Services.AddScoped<ITransaccionService, TransaccionService>();

builder.Services.AddScoped<IPasswordHasher<Usuario>,PasswordHasher<Usuario>>();

// Servicio global en toda la aplicación que aplica para los enums, diciendo que en vez de utilizar su número entero como 0 o 1, obtendrá el valor (nombre) y lo convertirá por medio
// del new JsonStringEnumConverter(JsonNamingPolicy.CamelCase).
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters
        .Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues:false)); //El allowIntegerValues:false lo agregamos para evitar jugar con las posiciones del enum por si en algun futuro se agrega uno y se cambia el orden
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:50907", "http://localhost:5261")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

//Registramos nuestro Middleware antes de los endpoints porque el orden del pipeline importa
app.UseMiddleware<ExceptionHandlingMiddleware>();

//Activamos el cors que inyectamos en la parte de arriba
app.UseCors("BlazorClient");

//Agregamos las rutas de los endpoints
app.MapUsuarioEndpoints();

app.MapCuentaEndpoints();

app.MapCategoriaEndpoints();

app.MapTransaccionEndpoints();

app.Run();
