using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PoyectoPokedexApi.Pages.Pokemon
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Eliminar la sesión
            HttpContext.Session.Remove("IsAuthenticated");
            HttpContext.Session.Remove("Usuario");

            // Redirigir a la página de inicio o login
            return RedirectToPage("/Pokemon/Login");
        }
    }
}
