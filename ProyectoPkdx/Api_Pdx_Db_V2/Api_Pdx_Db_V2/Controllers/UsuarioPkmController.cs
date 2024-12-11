using Api_Pdx_Db_V2.Data;
using Api_Pdx_Db_V2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Pdx_Db_V2.Controllers
{
    [ApiController]
    [Route("Api_Pdx_DbV2/[controller]")]
    public class UsuarioPkmController : ControllerBase
    {
        private readonly DbConexionContext _conexionContext;
        public UsuarioPkmController(DbConexionContext conexionContext)
        {
            _conexionContext = conexionContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UsuarioPkmModel>> GetUsuarios_pkm()
        {
            return Ok(_conexionContext.usuario_pkm.ToList());
        }
        //funcioal
        [HttpGet("{_idUsuario}")]
        public ActionResult<IEnumerable<UsuarioPkmModel>> ObtenerPkmsUsuario(int _idUsuario)
        {
            var datos = _conexionContext.usuario_pkm.Where(up => up.IdUsuario == _idUsuario)
            .ToList();
            if (datos == null || !datos.Any())
            {
                return NotFound("Datos no encontrados");
            }
            return Ok(datos);
        }

        [HttpPost("AgregarUsuarioPkm")]
        public async Task<IActionResult>agreagrUsuarioPkm([FromBody] UsuarioPkmModel usuarioPkm, [FromServices] PokeCliet pokeCliet)
        {
            if (usuarioPkm == null)
            {
                return BadRequest("Los datos del Pokémon son inválidos.");
            }

            // Validar que el IdUsuario exista en la tabla Usuario
            var usuario = await _conexionContext.usuario
                .FirstOrDefaultAsync(u => u.Id == usuarioPkm.IdUsuario);

            if (usuario == null)
            {
                return BadRequest($"El usuario con ID {usuarioPkm.IdUsuario} no existe.");
            }

            

            try
            {
                // Obtener los datos del Pokémon desde la API
                var pokemonData = await pokeCliet.GetPokemon(usuarioPkm.pkm_id.ToString());
                if(pokemonData == null)
                {
                    return BadRequest("Datos no econtrados");
                }
                // Asignar siempre el estado 1
                usuarioPkm.estado = 1;

                // Crear la entidad para la base de datos
                var nuevoUsuarioPkm = new UsuarioPkmModel
                {
                    IdUsuario = usuarioPkm.IdUsuario,
                    pkm_id = pokemonData.id,
                    nombre = pokemonData.name,
                    estado = usuarioPkm.estado  // Este valor será siempre 1
                };

                // Insertar el nuevo UsuarioPkm en la base de datos
                _conexionContext.usuario_pkm.Add(nuevoUsuarioPkm);
                await _conexionContext.SaveChangesAsync();

                // Devolver la respuesta con el objeto recién creado
                return Ok("Pokemon agregado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el usuario Pokémon: {ex.Message}");
            }
        }


        [HttpDelete("EliminarUsuarioPkm/{idUsuario}/{idPkm}/{idUsuarioPkm}")]
        public async Task<IActionResult> EliminarUsuarioPkm(int idUsuario, int idPkm, int idUsuarioPkm)
        {
            try
            {
                // Buscar el registro en la tabla usuario_pkm que coincida con todos los IDs proporcionados
                var usuarioPkm = await _conexionContext.usuario_pkm
                    .FirstOrDefaultAsync(up => up.IdUsuario == idUsuario && up.pkm_id == idPkm && up.Id == idUsuarioPkm);

                // Validar si se encontró el registro
                if (usuarioPkm == null)
                {
                    return NotFound($"No se encontró el Pokémon con ID {idPkm}, asociado al usuario con ID {idUsuario}, en el registro {idUsuarioPkm}.");
                }

                // Eliminar el registro de la base de datos
                _conexionContext.usuario_pkm.Remove(usuarioPkm);
                await _conexionContext.SaveChangesAsync();

                // Devolver respuesta de éxito
                return NoContent(); // Respuesta sin contenido (204)
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el Pokémon: {ex.Message}");
            }
        }
    }
}
