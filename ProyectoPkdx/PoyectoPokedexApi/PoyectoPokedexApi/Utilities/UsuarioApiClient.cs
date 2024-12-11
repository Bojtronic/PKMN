using Microsoft.AspNetCore.Mvc;
using PoyectoPokedexApi.Models;

namespace PoyectoPokedexApi.Utilities
{
    public class UsuarioApiClient
    {
        private readonly HttpClient _httpClient;

        public UsuarioApiClient(HttpClient httpClient) 
        { 
            _httpClient = httpClient;
        }
        public async Task<LoginModel> LoginAsync(LoginModel login)
        {
            var response = await _httpClient.PostAsJsonAsync("login", login);
            if (response.IsSuccessStatusCode)
            {
                // Si la respuesta es exitosa, deserializar la respuesta JSON
                var responseData = await response.Content.ReadFromJsonAsync<LoginModel>();
                return responseData;
            }
            throw new Exception($"Error al realizar login: {response.ReasonPhrase}");
        }
    }
}
