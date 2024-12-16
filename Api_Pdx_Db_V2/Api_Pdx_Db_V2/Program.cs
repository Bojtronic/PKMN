using Microsoft.EntityFrameworkCore;
using Api_Pdx_Db_V2.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("https://localhost:7232") // Cambia el puerto según el de tu aplicación web
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Agregar servicios de DbContext y otras dependencias
var connectionString = builder.Configuration.GetConnectionString("AccesoConexion");
builder.Services.AddDbContext<DbConexionContext>(
    options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddHttpClient<PokeCliet>(); // Si usas este cliente, asegúrate de que esté bien configurado

builder.Services.AddControllers(); // Para la API de Controladores
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Habilitar Swagger solo en Desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configurar middleware
app.UseHttpsRedirection();

// Habilitar CORS antes de UseAuthorization
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers(); // Mapeo de controladores API

app.Run();
