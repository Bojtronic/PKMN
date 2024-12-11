using Microsoft.AspNetCore.Mvc.RazorPages;
using PoyectoPokedexApi.Models.PokemonModel;
using PoyectoPokedexApi.Utilities;

namespace PoyectoPokedexApi.Pages.Pokemon
{
    public class IndexModel : PageModel
    {
        private readonly PokeClient _pokeClient;
         
        public PokemonModel pokemon { get; set; }

        public IndexModel(PokeClient pokeClient)
        {
            _pokeClient = pokeClient;
        }

        public async Task OnGetAsync()
        {
            pokemon = await _pokeClient.GetPokemon("1");
        }
       
       
    }
}
