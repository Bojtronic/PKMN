using PoyectoPokedexApi.Models.UsuarioModel;
using System.Text;
using Newtonsoft.Json;
using PoyectoPokedexApi.Models.PokemonModel;


namespace PoyectoPokedexApi.Utilities
{
    public class UsuarioApiClient
    {
        private readonly HttpClient _httpClient;

        public UsuarioApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public static async Task EjecutarTask()
        {
            // URL de la API
            string url = "https://localhost:7068/Api_Pdx_DbV2/Conexion";

            try
            {
                // Crear instancia de HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Configurar el encabezado
                    client.DefaultRequestHeaders.Add("Accept", "*/*");

                    // Llamar a la API
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el cuerpo de la respuesta
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Respuesta de la API: {responseBody}");
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al llamar a la API: {ex.Message}");
            }
        }

        public async Task<LogResponse> LoginUsuario(LogModel login)
        {
            // Serializamos el objeto login en formato JSON
            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");

            // Realizamos la solicitud POST al API de login
            var response = await _httpClient.PostAsync("https://localhost:7068/Api_Pdx_DbV2/Usuario/login", content);

            if (response.IsSuccessStatusCode)
            {
                // Leemos la respuesta de la API
                var responseBody = await response.Content.ReadAsStringAsync();

                // Deserializamos la respuesta en un objeto LogResponse
                var result = JsonConvert.DeserializeObject<LogResponse>(responseBody);

                // Retornamos el resultado
                return result;
            }

            return null; // Retorna null si la solicitud no es exitosa
        }

        public async Task<IEnumerable<UsuarioPkmModel>> ObtenerPkmsUsuario(int idUsuario)
        {
            // Construir la URL con el idUsuario
            string url = $"https://localhost:7068/Api_Pdx_DbV2/UsuarioPkm/{idUsuario}";

            try
            {
                // Realizar la solicitud GET
                var response = await _httpClient.GetAsync(url);

                // Verificar si la respuesta es exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer el cuerpo de la respuesta como string
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserializar la respuesta en una lista de UsuarioPkmModel
                    var datos = JsonConvert.DeserializeObject<IEnumerable<UsuarioPkmModel>>(responseBody);

                    if (datos == null)
                    {
                        Console.WriteLine("No se pudieron deserializar los datos.");
                        return new List<UsuarioPkmModel>(); // Retornar una lista vacía en caso de que los datos no se puedan deserializar
                    }

                    return datos; // Retornar los datos deserializados
                }
                else
                {
                    // Manejar el error si la respuesta no es exitosa
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return new List<UsuarioPkmModel>(); // Retornar una lista vacía en caso de error
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones más robusto
                Console.WriteLine($"Excepción al llamar a la API: {ex.Message}");
                return new List<UsuarioPkmModel>(); // Retornar una lista vacía en caso de error
            }
        }


        public async Task<EstadoModel> ObtenerEstado(int id)
        {
            string url = $"https://localhost:7068/Api_Pdx_DbV2/Estado/estado/{id}";
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Respuesta de la API: {responseBody}");
                    var datos = JsonConvert.DeserializeObject<EstadoModel>(responseBody);
                    return datos;
                }
                else
                {
                    Console.WriteLine($"Error en la API: {response.StatusCode} - {response.ReasonPhrase}");
                    return null; // Asegura un retorno aquí
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al llamar a la API: {ex}");
                return null; // Retorno en caso de excepción
            }
        }

        public async Task<bool> AgregarPokedexAsync(int userId, int pokemonId)
        {
            var url = $"https://localhost:7068/Api_Pdx_DbV2/UsuarioPkm/AgregarPkm/{userId}/{pokemonId}";

            // Crea el contenido de la solicitud POST
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, content);

                // Devuelve true si la solicitud fue exitosa
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar el Pokémon: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ReemplazarPokemonAsync(int idUsuario, int pkmId1, int pkmId2)
        {
            var url = $"https://localhost:7068/Api_Pdx_DbV2/UsuarioPkt/intercambiar?idUsuario={idUsuario}&pkmId1={pkmId1}&pkmId2={pkmId2}";
            var response = await _httpClient.PutAsync(url, null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CrearUsuarioAsync(CreateModel nuevoUsuario)
        {
            var url = "https://localhost:7068/Api_Pdx_DbV2/Usuario/Crear Usuario";

            // Serializamos el objeto nuevoUsuario en formato JSON
            var content = new StringContent(JsonConvert.SerializeObject(nuevoUsuario), Encoding.UTF8, "application/json");

            try
            {
                // Realizamos la solicitud POST al API de creación de usuario
                var response = await _httpClient.PostAsync(url, content);

                // Devuelve true si la solicitud fue exitosa
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el usuario: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> AsignarPktAsync(int idUsuario, int idPokemon)
        {
            var url = $"https://localhost:7068/Api_Pdx_DbV2/UsuarioPkt/AsignarPocket?_idUsuario={idUsuario}&_idPokemon={idPokemon}";

            try
            {
                var response = await _httpClient.PostAsync(url, null);

                // Devuelve true si la solicitud fue exitosa
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al asignar el Pokémon: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CrearReto(int IdUser1, int IdUser2)
        {
            var url = $"https://localhost:7068/Api_Pdx_DbV2/Retos/CrearReto/{IdUser1}/{IdUser2}";
            try
            {
                var response = await _httpClient.PostAsync(url, null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
