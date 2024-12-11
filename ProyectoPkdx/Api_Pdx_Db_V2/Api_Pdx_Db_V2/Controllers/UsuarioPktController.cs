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
        [HttpPost("Asignar-pocket")]
        public async Task<ActionResult> asignarPocketUsuario(int _idUsuario) 
        {
            //busca lo datos de la lista pokemo user 
            var pkmUsuario = await _conexionContext.usuario_pkm
            .Where(up => up.IdUsuario == _idUsuario && up.estado == 1)
            .ToListAsync();
            
            if (pkmUsuario.Count < 3) {
                return BadRequest("Se necesitan al menos 3 pokemnon para asignar");
            }

            //agrega los primeros 3 al poket
            var pkmSeleccionados = pkmUsuario.Take(3).ToList();

            foreach (var pkm in pkmSeleccionados)
            {
               pkm.estado = 2; // Cambiar a estado "Poket"
            }
            // Crear un nuevo pocket
            
            var nuevoPocket = new UsuarioPktModel
            {
                IdUsuario = _idUsuario,
                pkm_Id1 = pkmSeleccionados[0].Id,
                pkm_Id2 = pkmSeleccionados[1].Id,
                pkm_Id3 = pkmSeleccionados[2].Id

            };
            try
            {
                // Agregar el nuevo pocket a la base de datos
                _conexionContext.usuario_pocket.Add(nuevoPocket);
                // Guardar los cambios en la base de datos
                await _conexionContext.SaveChangesAsync();
                return Ok("Pocket agregado.");
            }
            catch (Exception ex)
            {
                // Manejo de errores
                 return BadRequest($"Error al agregar el pocket: {ex.Message}");
            }
            //_conexionContext.usuario_pocket.Add(nuevoPocket);

            //return Ok("Poket agregado");
        }
        //busca por id

        [HttpPut("remplazar-pokemon")]
        public ActionResult ReemplazarPokt(int idUsuario, int pkmIdRemplazar, int idPkmNuevo)
        {
            var pocket = _conexionContext.usuario_pocket.FirstOrDefault(up => up.IdUsuario == idUsuario);
            if (pocket == null)
            {
                return NotFound("Pocket no encontrado para el usuario.");
            }
            // Verificar si el Pokémon a reemplazar está en el pocket

            if (pocket.pkm_Id1 == pkmIdRemplazar)
            {
                pocket.pkm_Id1 = idPkmNuevo;
            }
            else if (pocket.pkm_Id2 == pkmIdRemplazar)
            {
                pocket.pkm_Id2 = idPkmNuevo;
            }
            else if (pocket.pkm_Id3 == pkmIdRemplazar)
            {
                pocket.pkm_Id3 = idPkmNuevo;
            }
            else
            {
                return BadRequest("El Pokémon a reemplazar no se encuentra en el pocket.");
            }

            var pkmReemplazado = _conexionContext.usuario_pkm.Find(pkmIdRemplazar);
            if(pkmReemplazado != null)
            {
                pkmReemplazado.estado = 1;
            }
            var pkmNuevo = _conexionContext.usuario_pkm.Find(idPkmNuevo);
            if (pkmNuevo != null)
            {
                pkmNuevo.estado = 2; // Cambiar a estado "Poket"
            }
            _conexionContext.SaveChanges();
            return Ok("Pokémon reemplazado con éxito.");
        }
    }
}
