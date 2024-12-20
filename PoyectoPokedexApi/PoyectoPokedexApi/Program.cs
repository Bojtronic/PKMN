using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PoyectoPokedexApi.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios a la aplicaci�n
//builder.Services.AddRazorPages(); // Si tienes Razor Pages
builder.Services.AddHttpClient<PokeClient>(); // Cliente para la API de Pok�mon

// Agregar cliente HTTP para la API
builder.Services.AddHttpClient<UsuarioApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7068/Api_Pdx_DbV2/"); // Base URL de la API
});


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // tiempo de expiraci�n de la sesi�n
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews()
    .AddCookieTempDataProvider();

var connectionString = builder.Configuration.GetConnectionString("AccesoConexion");

// Configuraci�n de DbContext si la aplicaci�n usa base de datos
// builder.Services.AddDbContext<DbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

app.UseStaticFiles();

// Configurar la carpeta Resources para servir archivos est�ticos
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
    RequestPath = "/Resources"  // Resources/Logo.jpg
});

// Configurar el manejo de errores en producci�n
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSession();

app.UseRouting();
app.UseHttpsRedirection(); 
app.UseAuthorization(); 

app.MapRazorPages();

app.Run();
