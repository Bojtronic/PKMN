using Api_Pdx_Db_V2.Data;
using Api_Pdx_Db_V2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api_Pdx_Db_V2.Controllers
{
    [ApiController]
    [Route("Api_Pdx_DbV2/[controller]")]
    public class EstadoController : ControllerBase
    {
        private readonly DbConexionContext _conexionContext;

        public EstadoController(DbConexionContext conexionContext)
        {
            _conexionContext = conexionContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EstadoModel>> GetEstado()
        {
            return Ok(_conexionContext.estado.ToList());
        }

        [HttpGet("estado/{id}")]
        public ActionResult<EstadoModel> GetEstadoPorId(int id)
        {
            var estado = _conexionContext.estado.Find(id);

            if (estado == null)
            {
                return NotFound("Estado no encontrado.");
            }
            return Ok(estado);
        }
    }
}
