using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PoyectoPokedexApi.Models.UsuarioModel;
using PoyectoPokedexApi.Utilities;

namespace PoyectoPokedexApi.Pages.Pokemon
{
    public class RegistroModel : PageModel
    {
        private readonly UsuarioApiClient _usuarioApiClient;

        [BindProperty]
        public UsuarioModel usuario { get; set; }

        public UsuarioModel nuevoUsuario { get; set; }
        public RegistroModel(UsuarioApiClient usuarioApiClient)
        {
            _usuarioApiClient = usuarioApiClient;
            
        }
         public string MensajeRegistro { get; private set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Crear un nuevo usuario con las credenciales que ingresa el usuario
                nuevoUsuario = new UsuarioModel
                {
                    Id = 0,
                    Nombre = usuario.UserName, 
                    UserName = usuario.UserName,
                    Pass = usuario.Pass 
                };

                bool resultado = await _usuarioApiClient.CrearUsuarioAsync(nuevoUsuario);
                if (resultado)
                {
                    MensajeRegistro = "Usuario creado exitosamente. Por favor, inicie sesión.";
                    return RedirectToPage("/Pokemon/Login"); // Redirigir a la página de inicio de sesión
                }
                else
                {
                    MensajeRegistro = "Error al crear el usuario.";
                }
            }
            catch (Exception ex)
            {
                MensajeRegistro = $"Error al intentar crear usuario: {ex.Message}";
            }

            return Page();
        }

    }
}

