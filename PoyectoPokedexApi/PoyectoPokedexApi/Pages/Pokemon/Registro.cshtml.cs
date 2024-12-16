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
        public LogModel Login { get; set; }

        public CreateModel nuevoUsuario { get; set; }
        public RegistroModel(UsuarioApiClient usuarioApiClient)
        {
            _usuarioApiClient = usuarioApiClient;
            
        }
         public string MensajeLogin { get; private set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Crear un nuevo usuario con las credenciales proporcionadas
                nuevoUsuario = new CreateModel
                {
                    Id = 0,
                    Nombre = Login.UserName, // Asumiendo que el nombre es el mismo que el nombre de usuario
                    UserName = Login.UserName,
                    Pass = Login.Password // Asumiendo que la clase UsuarioModel tiene una propiedad Pass
                };

                bool resultado = await _usuarioApiClient.CrearUsuarioAsync(nuevoUsuario);
                if (resultado)
                {
                    MensajeLogin = "Usuario creado exitosamente. Por favor, inicie sesión.";
                    return RedirectToPage("/Pokemon/Login"); // Redirigir a la página de inicio de sesión
                }
                else
                {
                    MensajeLogin = "Error al crear el usuario.";
                }
            }
            catch (Exception ex)
            {
                MensajeLogin = $"Error al intentar crear usuario: {ex.Message}";
            }

            return Page();
        }

    }
}
