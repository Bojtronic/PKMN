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

        [HttpPost("AgregarPkm/{idUsuario}/{idPkm}")]
        public async Task<IActionResult> AgregarUsuarioPkm(int idUsuario, int idPkm, [FromServices] PokeCliet pokeCliet)
        {
            if (idUsuario <= 0 || idPkm <= 0)
            {
                return BadRequest("Los valores de los parámetros son inválidos.");
            }

            // Validar que el IdUsuario exista en la tabla Usuario
            var usuario = await _conexionContext.usuario
                .FirstOrDefaultAsync(u => u.Id == idUsuario);

            if (usuario == null)
            {
                return BadRequest($"El usuario con ID {idUsuario} no existe.");
            }

            try
            {
                // Obtener los datos del Pokémon desde la API
                var pokemonData = await pokeCliet.GetPokemon(idPkm.ToString());
                if (pokemonData == null)
                {
                    return BadRequest("Datos del Pokémon no encontrados.");
                }

                // Asignar siempre el estado 1
                var usuarioPkm = new UsuarioPkmModel
                {
                    IdUsuario = idUsuario,
                    pkm_id = pokemonData.id,
                    nombre = pokemonData.name,
                    estado = 1 // Este valor será siempre 1
                };

                // Insertar el nuevo UsuarioPkm en la base de datos
                _conexionContext.usuario_pkm.Add(usuarioPkm);
                await _conexionContext.SaveChangesAsync();

                // Devolver la respuesta con el objeto recién creado
                return Ok("Pokémon agregado correctamente");
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

        [HttpGet("{_idUsuario}/PkmActivo")]
        public ActionResult<IEnumerable<UsuarioPkmModel>> ObtenerPkmsUsuarioConEstado(int _idUsuario)
        {
            var datos = _conexionContext.usuario_pkm
                .Where(up => up.IdUsuario == _idUsuario && up.estado == 1)
                .ToList();

            if (datos == null || !datos.Any())
            {
                return NotFound("No se encontraron datos con el estado 1 para este usuario.");
            }

            return Ok(datos);
        }


        [HttpGet("ObtenerPorEstado/{estado}")]
        public async Task<IActionResult> ObtenerPorEstado(int estado)
        {
            try
            {
                var resultados = await _conexionContext.usuario_pkm
                    .Where(up => up.estado == estado)
                    .Join(_conexionContext.usuario,
                        up => up.IdUsuario,
                        u => u.Id,
                        (up, u) => new UsuarioPkmConNombreUsuario
                        {
                            Id = up.Id,
                            IdUsuario = up.IdUsuario,
                            pkm_id = up.pkm_id,
                            nombre = up.nombre,
                            estado = up.estado,
                            NombreUsuario = u.Nombre
                        })
                    .ToListAsync();

                if (resultados == null || !resultados.Any())
                {
                    return Ok(new List<object>()); // Devuelve una lista vacía
                }

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los registros: {ex.Message}");
            }
        }

        [HttpPut("CurarPokemon/{id}")]
        public async Task<IActionResult> CurarPokemon(int id)
        {
            try
            {
                var usuarioPkm = await _conexionContext.usuario_pkm.FirstOrDefaultAsync(up => up.Id == id);

                if (usuarioPkm == null)
                {
                    return NotFound($"No se encontró el Pokémon con ID {id}.");
                }

                usuarioPkm.estado = 1;

                _conexionContext.usuario_pkm.Update(usuarioPkm);
                await _conexionContext.SaveChangesAsync();

                return Ok($"El Pokémon con ID {id} ha sido curado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al curar el Pokémon: {ex.Message}");
            }
        }


    }
}
