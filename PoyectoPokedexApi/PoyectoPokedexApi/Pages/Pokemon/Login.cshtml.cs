using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PoyectoPokedexApi.Models.UsuarioModel;
using PoyectoPokedexApi.Utilities;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;

namespace PoyectoPokedexApi.Pages.Pokemon
{
    public class LoginModel : PageModel
    {
        private readonly UsuarioApiClient _usuarioApiClient;

        // Propiedad para mostrar mensajes de estado en la vista
        public string MensajeLogin { get; private set; }

        public UsuarioModel UsuarioAutenticado { get; private set; }

        // Utiliza LogModel para recibir las credenciales de inicio de sesi�n
        [BindProperty]
        public LogModel Login { get; set; }

        public LoginModel(UsuarioApiClient usuarioApiClient)
        {
            _usuarioApiClient = usuarioApiClient;
            Login = new LogModel();
        }

        public void OnGet()
        {
            // Carga inicial de la p�gina
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var logResponse = await _usuarioApiClient.LoginUsuario(Login);

                if (logResponse != null && logResponse.Mensaje == "Login exitoso")
                {
                    UsuarioAutenticado = logResponse.Usuario;
                    MensajeLogin = "Inicio de sesi�n exitoso.";

                   
                    TempData["Usuario"] = JsonConvert.SerializeObject(logResponse.Usuario);
                    TempData["IsAuthenticated"] = true;

                    // Usando Session en su lugar:
                    HttpContext.Session.SetString("IsAuthenticated", "true");
                    HttpContext.Session.SetString("Usuario", JsonConvert.SerializeObject(logResponse.Usuario));

                    // Redirigir a la p�gina de User
                    return RedirectToPage("/Pokemon/User");
                }
                else
                {
                    MensajeLogin = "Credenciales inv�lidas.";
                    // Comentado el uso de TempData:
                    // TempData["IsAuthenticated"] = false;
                    HttpContext.Session.SetString("IsAuthenticated", "false");
                }
            }
            catch (Exception ex)
            {
                MensajeLogin = $"Error al intentar iniciar sesi�n: {ex.Message}";
            }

            return Page();
        }
    }
}
