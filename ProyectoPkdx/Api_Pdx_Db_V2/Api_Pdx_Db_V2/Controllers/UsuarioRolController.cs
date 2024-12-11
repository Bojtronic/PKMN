using Api_Pdx_Db_V2.Data;
using Api_Pdx_Db_V2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api_Pdx_Db_V2.Controllers
{
    [ApiController]
    [Route("Api_Pdx_DbV2/[controller]")]
    public class UsuarioRolController : ControllerBase
    {
        private readonly DbConexionContext _conexionContext;
        public UsuarioRolController(DbConexionContext conexionContext)
        {
            _conexionContext = conexionContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UsuarioRolModel>> GetUsuariosRoles()
        {
            return Ok(_conexionContext.usuario_rol.ToList());
        }

        [HttpPut("{idUsuario}")]
        public ActionResult CambiarRolDeUsuario(int idUsuario, [FromBody] int nuevoIdRol)
        {
            // Buscar la relación entre el usuario y su rol actual
            var usuarioRol = _conexionContext.usuario_rol.FirstOrDefault(ur => ur.IdUsuario == idUsuario);

            if (usuarioRol == null)
            {
                return NotFound("El usuario no tiene roles asignados.");
            }

            // Verificar si el nuevo rol existe
            var nuevoRol = _conexionContext.rol.FirstOrDefault(r => r.Id == nuevoIdRol);
            if (nuevoRol == null)
            {
                return NotFound("El nuevo rol no existe.");
            }

            // Actualizar el rol del usuario
            usuarioRol.IdRol = nuevoIdRol;

            // Guardar los cambios
            _conexionContext.SaveChanges();

            return Ok("Rol del usuario actualizado exitosamente.");


        }
    }
}
