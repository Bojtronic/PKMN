using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PoyectoPokedexApi.Utilities;

namespace PoyectoPokedexApi.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UsuarioApiClient _usuarioApiClient;
        private readonly ILogger<IndexModel> _logger;

        // Propiedad para almacenar el resultado de la conexión
        public string ConexionResultado { get; set; }

        public IndexModel(ILogger<IndexModel> logger, UsuarioApiClient usuarioApiClient)
        {
            _logger = logger;
            _usuarioApiClient = usuarioApiClient;
        }

        // Método manejador para validar la conexión
        public async Task<IActionResult> OnGetConexion()
        {
            try
            {
                // Llama al método que realiza la solicitud
                await UsuarioApiClient.EjecutarTask(); 
                return new JsonResult("Conexion Exitosa");
            }
            catch (Exception ex)
            {
                return new JsonResult($"Error: {ex.Message}");
            }
        }
    }
}
