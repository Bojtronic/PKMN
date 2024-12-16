using PoyectoPokedexApi.Models.PokemonModel;
using System.Text;
using System.Text.Json;

namespace PoyectoPokedexApi.Utilities
{
    public class PokeClient
    {

        private readonly HttpClient _httpClient;

        public PokeClient(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }
        public async Task<PokemonModel> GetPokemon(String id)
        {
            var response = await _httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al obtener datos del Pokémon. Código de estado: {response.StatusCode}, Razón: {response.ReasonPhrase}");
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PokemonModel>(responseContent)
                  ?? throw new Exception("La respuesta no tiene un formato válido para PokemonModel.");
        }

        public async Task<List<PokemonModel>> GetPokemons(List<string> ids)
        {
            var pokemons = new List<PokemonModel>();

            foreach (var id in ids)
            {
                var pokemon = await GetPokemon(id); // Usamos el método que ya tienes para obtener un Pokémon
                pokemons.Add(pokemon);
            }

            return pokemons;
        }
    }
}
