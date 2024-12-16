using Api_Pdx_Db_V2.Data;
using Api_Pdx_Db_V2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Pdx_Db_V2.Controllers
{
    [ApiController]
    [Route("Api_Pdx_DbV2/[controller]")]
    public class enfermeriaController : Controller
    {
        private readonly DbConexionContext _conexionContext;
        public enfermeriaController(DbConexionContext conexionContext)
        {
            _conexionContext = conexionContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EnfermeriaModel>> GetRoles()
        {
            return Ok(_conexionContext.enfermeria.ToList());
        }

        [HttpPost("RegistrarEnfermeria")]
        public async Task<ActionResult> RegistrarEnfermeria(int userId)
        {
            var pokUsuario1 = await _conexionContext.usuario_pocket
                .Where(p => p.IdUsuario == userId)
                .ToListAsync();

            if (!pokUsuario1.Any())
            {
                return BadRequest("El usuario 1 no tiene Pokémon en estado válido para participar.");
            }

            var idsPokemonsUsuario = pokUsuario1.Select(p => new { p.pkm_Id1, p.pkm_Id2, p.pkm_Id3 }).FirstOrDefault();

            if (idsPokemonsUsuario != null)
            {
                var pokeCurado = await _conexionContext.usuario_pkm
                    .Where(p => p.IdUsuario == userId &&
                                (p.Id == idsPokemonsUsuario.pkm_Id1 ||
                                 p.Id == idsPokemonsUsuario.pkm_Id2 ||
                                 p.Id == idsPokemonsUsuario.pkm_Id3))
                    .ToListAsync();

                foreach (var pkm in pokeCurado)
                {
                    pkm.estado = 2; // Estado 'poket'
                    // Verificar si el pkm_id existe en usuario_pkm
                    var existePkm = await _conexionContext.usuario_pkm
                        .Where(p => p.Id == pkm.Id && p.IdUsuario == userId)    
                        .FirstOrDefaultAsync();
                    if (existePkm == null)
                    {
                        return BadRequest($"El Pokémon con ID {pkm.pkm_id} no existe en la tabla usuario_pkm.");
                    }
                    var registroEnfermeria = new EnfermeriaModel
                    {
                        Idusuario = userId,
                        IdUsuPkm = pkm.Id, // Asegúrate de que este es el campo correcto
                        Descripcion = "Pokemon curado exitosamente",
                        fecha = DateTime.Now
                    };

                    _conexionContext.enfermeria.Add(registroEnfermeria);
                }
            }
           
            // Guardar los cambios en la base de datos
            await _conexionContext.SaveChangesAsync();

            return Ok($"Pokémon debilitados del usuario con ID {userId} registrados en enfermería y actualizados a estado 'Poket' exitosamente.");


        }
    }
}
