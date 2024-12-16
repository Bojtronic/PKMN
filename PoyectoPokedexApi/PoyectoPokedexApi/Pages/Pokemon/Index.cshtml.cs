using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PoyectoPokedexApi.Models.PokemonModel;
using PoyectoPokedexApi.Models.UsuarioModel;
using PoyectoPokedexApi.Utilities;

namespace PoyectoPokedexApi.Pages.Pokemon
{
    public class IndexModel : PageModel
    {
        private readonly PokeClient _pokeClient;
        

        public UsuarioModel Usuario { get; set; }
        public bool IsAuthenticated { get; set; }
        
        public PokemonModel pokemon { get; set; }
        public List<PokemonModel> pokemonList { get; set; }

        public IndexModel(PokeClient pokeClient)
        {
            
            _pokeClient = pokeClient;
        }

        public async Task OnGetAsync()
        {
            

      

            pokemon = await _pokeClient.GetPokemon("1");

            var pokemonIds = new List<string>();
            for (int i = 1; i <= 150; i++)  // Obtén los primeros 150 Pokémon
            {
                pokemonIds.Add(i.ToString());
            }

            pokemonList = await _pokeClient.GetPokemons(pokemonIds);  // Obtener los Pokémon desde la API
        }

       



    }
}
