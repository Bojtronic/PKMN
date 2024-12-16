using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PoyectoPokedexApi.Models.UsuarioModel;

namespace PoyectoPokedexApi.Pages.Enfermeria
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<PkmDebilitados> UsuariosPkm { get; set; }

        public async Task OnGet()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync("https://localhost:7068/Api_Pdx_DbV2/UsuarioPkm/ObtenerPorEstado/3");

            // Deserializar la respuesta JSON en una lista de objetos ViewModel
            UsuariosPkm = JsonConvert.DeserializeObject<List<PkmDebilitados>>(response);
        }
    }
}
