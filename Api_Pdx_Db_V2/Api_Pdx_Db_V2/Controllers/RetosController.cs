using Api_Pdx_Db_V2.Data;
using Api_Pdx_Db_V2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Pdx_Db_V2.Controllers
{
    [ApiController]
    [Route("Api_Pdx_DbV2/[controller]")]
    public class RetosController : ControllerBase
    {
        private readonly DbConexionContext _conexionContext;
        public RetosController(DbConexionContext conexionContext)
        {
            _conexionContext = conexionContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RetoModel>> GetRetos()
        {
            return Ok(_conexionContext.reto.ToList());
        }

        [HttpPut("CrearReto/{idUser1}/{idUser2}")]
        public async Task<ActionResult> CrearReto(int idUser1, int idUser2, [FromBody] RetoModel reto)
        {
            Console.WriteLine($"Recibido: idUser1={idUser1}, idUser2={idUser2}");

            // Obtener los usuarios desde la base de datos
            var usuario1 = await _conexionContext.usuario.FindAsync(idUser1);
            var usuario2 = await _conexionContext.usuario.FindAsync(idUser2);

            // Verificar si los usuarios existen
            if (usuario1 == null || usuario2 == null)
            {
                return NotFound("Uno o ambos usuarios no existen.");
            }

            var pokUsuario1 = await _conexionContext.usuario_pocket
                .Where(p => p.IdUsuario == idUser1)
                .ToListAsync();

            if (!pokUsuario1.Any())
            {
                return BadRequest("El usuario 1 no tiene Pokémon en estado válido para participar.");
            }

            var idsPokemonsUsuario1 = pokUsuario1.Select(p => new { p.pkm_Id1, p.pkm_Id2, p.pkm_Id3 }).FirstOrDefault();

            if (idsPokemonsUsuario1 != null)
            {
                var pokemonsUsuario1 = await _conexionContext.usuario_pkm
                    .Where(p => p.IdUsuario == idUser1 &&
                                (p.Id == idsPokemonsUsuario1.pkm_Id1 ||
                                 p.Id == idsPokemonsUsuario1.pkm_Id2 ||
                                 p.Id == idsPokemonsUsuario1.pkm_Id3) &&
                                p.estado == 3) // Estado "Debilitado" (estado = 3)
                    .ToListAsync();

                if (pokemonsUsuario1.Any())
                {
                    return BadRequest("El usuario 1 tiene Pokémon debilitados y no puede participar.");
                }
            }

            var pokUsuario2 = await _conexionContext.usuario_pocket
                .Where(p => p.IdUsuario == idUser2)
                .ToListAsync();

            if (!pokUsuario2.Any())
            {
                return BadRequest("El usuario 2 no tiene Pokémon en estado válido para participar.");
            }

            var idsPokemonsUsuario2 = pokUsuario2.Select(p => new { p.pkm_Id1, p.pkm_Id2, p.pkm_Id3 }).FirstOrDefault();

            if (idsPokemonsUsuario2 != null)
            {
                var pokemonsUsuario2 = await _conexionContext.usuario_pkm
                    .Where(p => p.IdUsuario == idUser2 &&
                                (p.Id == idsPokemonsUsuario2.pkm_Id1 ||
                                 p.Id == idsPokemonsUsuario2.pkm_Id2 ||
                                 p.Id == idsPokemonsUsuario2.pkm_Id3) &&
                                p.estado == 3) // Estado "Debilitado" (estado = 3)
                    .ToListAsync();

                if (pokemonsUsuario2.Any())
                {
                    return BadRequest("El usuario 2 tiene Pokémon debilitados y no puede participar.");
                }
            }

            // Seleccionar un mensaje aleatorio de la tabla mensajesPred
            var mensajeAleatorio = await _conexionContext.mensajes
                .OrderBy(m => EF.Functions.Random()) // Selección aleatoria
                .Select(m => m.Mensaje)
                .FirstOrDefaultAsync();

            if (mensajeAleatorio == null)
            {
                return StatusCode(500, "No se pudo seleccionar un mensaje aleatorio.");
            }

            // Seleccionar aleatoriamente un ganador
            var ganador = (new Random().Next(0, 2) == 0) ? idUser1 : idUser2;

            // Crear el objeto RetoModel y agregarlo a la base de datos
            var nuevoReto = new RetoModel
            {
                IdUsr1 = idUser1,
                IdUsr2 = idUser2,
                IdGanador = ganador,
                Mensaje = mensajeAleatorio
            };

            _conexionContext.reto.Add(nuevoReto);
            await _conexionContext.SaveChangesAsync();

            // Retornar una respuesta exitosa con detalles del reto
            return Ok($"Reto creado exitosamente entre Usuario {idUser1} y Usuario {idUser2} con mensaje: {mensajeAleatorio}");
        }

    }
}
