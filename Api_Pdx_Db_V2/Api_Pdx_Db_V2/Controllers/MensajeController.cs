using Api_Pdx_Db_V2.Data;
using Api_Pdx_Db_V2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api_Pdx_Db_V2.Controllers
{
    [ApiController]
    [Route("Api_Pdx_DbV2/[controller]")]
    public class MensajeController : ControllerBase
    {
        private readonly DbConexionContext _conexionContext;
        public MensajeController(DbConexionContext conexionContext)
        {
            _conexionContext = conexionContext;
        }
        [HttpGet("Mensaje")]
        public ActionResult<MensajesModel> GetMensajePorId(int id) 
        {
            var mensaje = _conexionContext.mensajes.FirstOrDefault(x => x.Id == id);
            if (mensaje == null)
            {
                return NotFound("Mensaje no exite");
            }
            return Ok(mensaje.Mensaje);
        }
    }
}
