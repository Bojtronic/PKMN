using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PoyectoPokedexApi.Models.UsuarioModel;
using PoyectoPokedexApi.Utilities;

namespace PoyectoPokedexApi.Pages.Pokemon
{
    public class UsuariosModel : PageModel
    {
        private readonly UsuarioApiClient _usuarioApiClient;

        public UsuariosModel(UsuarioApiClient usuarioApiClient)
        {
            _usuarioApiClient = usuarioApiClient;
        }

        // Lista de usuarios para mostrar en la vista
        public List<UsuarioModel> Usuarios { get; set; }

        // Acción para cargar los usuarios desde la API
        public async Task OnGetAsync()
        {
            // Obtener la lista de usuarios desde la API
            Usuarios = await _usuarioApiClient.ObtenerUsuariosAsync();
        }

        // Acción para crear un nuevo usuario
        public async Task<IActionResult> OnPostCrearUsuarioAsync(UsuarioModel nuevoUsuario)
        {
            bool resultado = await _usuarioApiClient.CrearUsuarioAsync(nuevoUsuario);
            if (resultado)
            {
                // Redirigir o mostrar mensaje de éxito
                return RedirectToPage("/Pokemon/Usuarios");
            }
            else
            {
                // Mostrar mensaje de error
                return Page();
            }
        }

        // Acción para editar un usuario existente
        public async Task<IActionResult> OnPostEditarUsuarioAsync(int idUsuario, UsuarioModel usuarioModificado)
        {
            bool resultado = await _usuarioApiClient.EditarUsuarioAsync(idUsuario, usuarioModificado);
            if (resultado)
            {
                // Redirigir o mostrar mensaje de éxito
                return RedirectToPage("/Pokemon/Usuarios");
            }
            else
            {
                // Mostrar mensaje de error
                return Page();
            }
        }

        // Acción para eliminar un usuario
        public async Task<IActionResult> OnPostEliminarUsuarioAsync(int idUsuario)
        {
            bool resultado = await _usuarioApiClient.EliminarUsuarioAsync(idUsuario);
            if (resultado)
            {
                // Redirigir o mostrar mensaje de éxito
                return RedirectToPage("/Pokemon/Usuarios");
            }
            else
            {
                // Mostrar mensaje de error
                return Page();
            }
        }
    }
}
