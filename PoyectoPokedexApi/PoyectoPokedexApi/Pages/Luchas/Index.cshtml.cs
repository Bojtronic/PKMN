using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PoyectoPokedexApi.Models.PokemonModel;
using PoyectoPokedexApi.Models.UsuarioModel;
using PoyectoPokedexApi.Utilities;

namespace PoyectoPokedexApi.Pages.Luchas
{
    public class IndexModel : PageModel
    {
        private readonly UsuarioApiClient _usuarioApiClient;

        private readonly PokeClient _pokeClient;
        public List<PokemonModel> pokemon { get; set; } = new List<PokemonModel>();
        public UsuarioModel Usuario { get; set; }
        public List<UsuarioPkmModel> UsuarioPkms { get; set; } = new List<UsuarioPkmModel>();
        public List<RetoModel> retos { get; set; } = new List<RetoModel>();


        [BindProperty]
        public int Pokemon1Id { get; set; }

        [BindProperty]
        public int Pokemon2Id { get; set; }

        [BindProperty]
        public int HiddenInput { get; set; } //este valor indica si perdió el jugador 1 o el 2, si hay empate es cero. con esto se decide el estado para ir a enfermeria

        [BindProperty]
        public int HiddenFinish { get; set; } //si está en 1 indica que la batalla termino


        public void OnPostAsignarPokemon_1(int pokemonId)
        {
            // Lógica para asignar el Pokémon al jugador 1
            Pokemon1Id = pokemonId;
        }

        public void OnPostAsignarPokemon_2(int pokemonId)
        {
            // Lógica para asignar el Pokémon al jugador 2
            Pokemon2Id = pokemonId;
        }

        public IndexModel(UsuarioApiClient usuarioApiClient, PokeClient pokeClient)
        {
            _usuarioApiClient = usuarioApiClient ?? throw new ArgumentNullException(nameof(usuarioApiClient));
            _pokeClient = pokeClient ?? throw new ArgumentNullException(nameof(pokeClient));
        }

        public async Task OnGet()
        {
            retos = new List<RetoModel>();
            pokemon = new List<PokemonModel>();

            // Obtener el objeto Usuario de TempData
            if (TempData["Usuario"] != null)
            {
                Usuario = JsonConvert.DeserializeObject<UsuarioModel>(TempData["Usuario"].ToString());

                if (Usuario != null)
                {
                    try
                    {
                        var usuarioPkmsResult = await _usuarioApiClient.ObtenerPkmsUsuario(Usuario.Id);
                        UsuarioPkms = usuarioPkmsResult != null ? (List<UsuarioPkmModel>)usuarioPkmsResult : new List<UsuarioPkmModel>();

                        // Iterar sobre los Pokémon del usuario y obtener detalles
                        foreach (var usuarioPkm in UsuarioPkms)
                        {
                            var estado = await _usuarioApiClient.ObtenerEstado(usuarioPkm.estado);
                            var pokemonDetail = await _pokeClient.GetPokemon(usuarioPkm.pkm_id.ToString());

                            pokemon.Add(new PokemonModel
                            {
                                id = usuarioPkm.pkm_id,
                                name = pokemonDetail.name,
                                sprites = pokemonDetail.sprites,
                                height = pokemonDetail.height,
                                weight = pokemonDetail.weight,
                                stats = pokemonDetail.stats,
                                types = pokemonDetail.types,
                                estado = estado.Estado
                            });
                        }

                        TempData["Usuario"] = JsonConvert.SerializeObject(Usuario);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al obtener los datos del usuario o Pokémon: {ex.Message}");
                        UsuarioPkms = new List<UsuarioPkmModel>();
                    }
                }
                else
                {
                    Console.WriteLine("Usuario es null después de deserialización.");
                }
            }
            else
            {
                Console.WriteLine("TempData[\"Usuario\"] es null. No se puede deserializar el usuario.");
                // Aquí puedes redirigir a una página de error o mostrar un mensaje
            }
        }

        

    }
}
