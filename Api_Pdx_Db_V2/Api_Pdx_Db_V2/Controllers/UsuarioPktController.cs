using Api_Pdx_Db_V2.Data;
using Api_Pdx_Db_V2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Pdx_Db_V2.Controllers
{
    [ApiController]
    [Route("Api_Pdx_DbV2/[controller]")]
    public class UsuarioPktController : ControllerBase
    {

        private readonly DbConexionContext _conexionContext;
        public UsuarioPktController(DbConexionContext conexionContext)
        {
            _conexionContext = conexionContext;
        }
        [HttpGet]
        //Trae todos los datos de tabla para comparar
        public ActionResult<IEnumerable<UsuarioPktModel>> GetUsuarios_pkt()
        {
            return Ok(_conexionContext.usuario_pocket.ToList());
        }

        //busca en el usuario seleccionado 

        [HttpGet("{_idUsuario}")]
        public ActionResult<IEnumerable<UsuarioPktModel>> ObtenerPktUsuario(int _idUsuario)
        {
            
            var pktUsuario = _conexionContext.usuario_pocket.Where(up => up.IdUsuario == _idUsuario).ToList();
            if (pktUsuario== null|| !pktUsuario.Any())
            {
                return NotFound("Datos no encontrados");
            }
            return Ok(pktUsuario);
        }

        //Asigna los primeros 3 pokemon regitrados de la tabla por usuario
        //por usuario para 
        [HttpPost("AsignarPocket")]
        public async Task<ActionResult> AsignarPocketUsuario(int _idUsuario, int _idPokemon)
        {
            // Verifica si el Pokémon existe y pertenece al usuario
            var pkmUsuario = await _conexionContext.usuario_pkm
                .FirstOrDefaultAsync(up => up.IdUsuario == _idUsuario && up.pkm_id == _idPokemon && up.estado == 1); // Estado "activo" (estado = 1)

            if (pkmUsuario == null)
            {
                return NotFound("El Pokémon no existe o no está activo.");
            }

            // Verifica si el pocket del usuario ya tiene Pokémon asignados
            var pocketExistente = await _conexionContext.usuario_pocket
                .FirstOrDefaultAsync(up => up.IdUsuario == _idUsuario);

            if (pocketExistente != null)
            {
                // Verifica si el pocket está lleno
                if (pocketExistente.pkm_Id1 != 0 && pocketExistente.pkm_Id2 != 0 && pocketExistente.pkm_Id3 != 0)
                {
                    return BadRequest("El pocket está lleno. ¿Desea reemplazar uno de los Pokémon?");
                }

                // Agrega el Pokémon al primer espacio disponible en el pocket
                if (pocketExistente.pkm_Id1 == 0)
                {
                    pocketExistente.pkm_Id1 = _idPokemon;
                }
                else if (pocketExistente.pkm_Id2 == 0)
                {
                    pocketExistente.pkm_Id2 = _idPokemon;
                }
                else if (pocketExistente.pkm_Id3 == 0)
                {
                    pocketExistente.pkm_Id3 = _idPokemon;
                }
            }
            else
            {
                // Crear un nuevo pocket si no existe
                var nuevoPocket = new UsuarioPktModel
                {
                    IdUsuario = _idUsuario,
                    pkm_Id1 = _idPokemon,
                    pkm_Id2 = 0,
                    pkm_Id3 = 0
                };

                _conexionContext.usuario_pocket.Add(nuevoPocket);
            }

            // Cambiar el estado del Pokémon a "Poket"
            pkmUsuario.estado = 2;

            try
            {
                // Guardar los cambios en la base de datos
                await _conexionContext.SaveChangesAsync();
                return Ok("Pokémon agregado al pocket.");
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return BadRequest($"Error al agregar el Pokémon al pocket: {ex.Message}");
            }
        }

        //busca por id

        [HttpPut("intercambiar")]
        public ActionResult IntercambiarPokemons(int idUsuario, int pkmId1, int pkmId2)
        {
            var pocket = _conexionContext.usuario_pocket.FirstOrDefault(up => up.IdUsuario == idUsuario);
            if (pocket == null)
            {
                return NotFound("Pocket no encontrado para el usuario.");
            }

            // Verificar si ambos Pokémon están en el pocket
            bool isPkm1InPocket = pocket.pkm_Id1 == pkmId1 || pocket.pkm_Id2 == pkmId1 || pocket.pkm_Id3 == pkmId1;
            bool isPkm2InPocket = pocket.pkm_Id1 == pkmId2 || pocket.pkm_Id2 == pkmId2 || pocket.pkm_Id3 == pkmId2;

            if (!isPkm1InPocket && !isPkm2InPocket)
            {
                return BadRequest("Ninguno de los Pokémon a intercambiar se encuentra en el pocket.");
            }

            // Intercambiar los Pokémon en el pocket
            if (pocket.pkm_Id1 == pkmId1)
            {
                pocket.pkm_Id1 = pkmId2;
            }
            else if (pocket.pkm_Id2 == pkmId1)
            {
                pocket.pkm_Id2 = pkmId2;
            }
            else if (pocket.pkm_Id3 == pkmId1)
            {
                pocket.pkm_Id3 = pkmId2;
            }

            if (pocket.pkm_Id1 == pkmId2)
            {
                pocket.pkm_Id1 = pkmId1;
            }
            else if (pocket.pkm_Id2 == pkmId2)
            {
                pocket.pkm_Id2 = pkmId1;
            }
            else if (pocket.pkm_Id3 == pkmId2)
            {
                pocket.pkm_Id3 = pkmId1;
            }

            // Actualizar el estado de los Pokémon en la base de datos
            var pkm1 = _conexionContext.usuario_pkm.FirstOrDefault(up => up.IdUsuario == idUsuario && up.pkm_id == pkmId1);
            var pkm2 = _conexionContext.usuario_pkm.FirstOrDefault(up => up.IdUsuario == idUsuario && up.pkm_id == pkmId2);

            if (pkm1 != null && pkm2 != null)
            {
                pkm1.estado = isPkm2InPocket ? 2 : 1; // Cambiar estado basado en la nueva ubicación
                pkm2.estado = isPkm1InPocket ? 2 : 1; // Cambiar estado basado en la nueva ubicación

                _conexionContext.usuario_pkm.Update(pkm1);
                _conexionContext.usuario_pkm.Update(pkm2);
            }
            else
            {
                return NotFound("Uno o ambos Pokémon no se encuentran en la base de datos.");
            }

            _conexionContext.usuario_pocket.Update(pocket);
            _conexionContext.SaveChanges();
            return Ok("Pokémon intercambiados con éxito.");
        }

    }
}
