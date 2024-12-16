using Api_Pdx_Db_V2.Data;
using Api_Pdx_Db_V2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Pdx_Db_V2.Controllers
{
    [ApiController]
    [Route("Api_Pdx_DbV2/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly DbConexionContext _conexionContext;
        public RolController (DbConexionContext conexionContext)
        {
            _conexionContext = conexionContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RolModel>> GetRoles() 
        {
            return Ok(_conexionContext.rol.ToList());
        }

        // Obtener un usuario por ID
        [HttpGet("rol/{id}")]
        public ActionResult<RolModel> GetRolPorId(int id)
        {
            var rol = _conexionContext.rol.Find(id);

            if (rol == null)
            {
                return NotFound("rol no encontrado.");
            }
            return Ok(rol.Descripcion);
        }
    }
}
