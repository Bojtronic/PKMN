using Api_Pdx_Db_V2.Models.PokemonModel;
using System.Text.Json;

namespace Api_Pdx_Db_V2.Data
{
    public class PokeCliet
    {
        private readonly HttpClient _httpClient;

        public PokeCliet(HttpClient httpClient)
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
    }
}
