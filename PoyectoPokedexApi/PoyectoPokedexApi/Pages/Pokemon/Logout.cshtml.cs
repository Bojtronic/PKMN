using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PoyectoPokedexApi.Pages.Pokemon
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Eliminar la sesi�n
            HttpContext.Session.Remove("IsAuthenticated");
            HttpContext.Session.Remove("Usuario");

            // Redirigir a la p�gina de inicio o login
            return RedirectToPage("/Pokemon/Login");
        }
    }
}
