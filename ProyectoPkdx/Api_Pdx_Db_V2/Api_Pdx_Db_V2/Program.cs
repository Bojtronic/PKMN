using Microsoft.EntityFrameworkCore;
using Api_Pdx_Db_V2.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<PokeCliet>();
var connectionString = builder.Configuration.GetConnectionString("AccesoConexion");


builder.Services.AddDbContext<DbConexionContext>(
    options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21))
    ));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
