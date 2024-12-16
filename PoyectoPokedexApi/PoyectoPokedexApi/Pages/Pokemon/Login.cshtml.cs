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

        // Utiliza LogModel para recibir las credenciales de inicio de sesión
        [BindProperty]
        public LogModel Login { get; set; }

        public LoginModel(UsuarioApiClient usuarioApiClient)
        {
            _usuarioApiClient = usuarioApiClient;
            Login = new LogModel();
        }

        public void OnGet()
        {
            // Carga inicial de la página
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
                    MensajeLogin = "Inicio de sesión exitoso.";

                    // Almacenar el objeto Usuario en TempData
                    TempData["Usuario"] = JsonConvert.SerializeObject(logResponse.Usuario);
                    TempData["IsAuthenticated"] = true;

                    // Redirigir a la página de User
                    return RedirectToPage("/Pokemon/User");
                }
                else
                {
                    MensajeLogin = "Credenciales inválidas.";
                    TempData["IsAuthenticated"] = false;
                }
            }
            catch (Exception ex)
            {
                MensajeLogin = $"Error al intentar iniciar sesión: {ex.Message}";
            }

            return Page();
        }

    }
}

