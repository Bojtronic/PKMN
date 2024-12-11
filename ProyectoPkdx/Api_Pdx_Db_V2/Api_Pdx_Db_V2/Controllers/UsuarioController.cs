using Api_Pdx_Db_V2.Data;
using Api_Pdx_Db_V2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;  // Necesario para operaciones asincrónicas
using BCrypt.Net;
using System.Threading.Tasks;

namespace Api_Pdx_Db_V2.Controllers
{
    [ApiController]
    [Route("Api_Pdx_DbV2/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly DbConexionContext _conexionContext;

        public UsuarioController(DbConexionContext conexionContext)
        {
            _conexionContext = conexionContext;
        }

        // Obtener todos los usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioModel>>> GetUsuario()
        {
            var usuarios = await _conexionContext.usuario.ToListAsync();
            return Ok(usuarios);
        }

        // Crear un nuevo usuario
        [HttpPost("Crear Usuario")]
        public async Task<ActionResult<UsuarioModel>> CrearUsuario([FromBody] UsuarioModel nuevoUsuario)
        {
            if (nuevoUsuario == null || string.IsNullOrWhiteSpace(nuevoUsuario.UserName) || string.IsNullOrWhiteSpace(nuevoUsuario.Pass))
            {
                return BadRequest("Los campos 'UserName' y 'Password' son obligatorios.");
            }

            // Encriptar la contraseña
            nuevoUsuario.Pass = BCrypt.Net.BCrypt.HashPassword(nuevoUsuario.Pass);
            _conexionContext.usuario.Add(nuevoUsuario);
            await _conexionContext.SaveChangesAsync();

            // Asignar rol al usuario
            var rol = await _conexionContext.rol.FirstOrDefaultAsync(r => r.Descripcion == "User");

            if (rol != null)
            {
                // Crear la relación en la tabla Usuario_Rol
                var usuarioRol = new UsuarioRolModel
                {
                    IdUsuario = nuevoUsuario.Id,
                    IdRol = rol.Id
                };

                _conexionContext.usuario_rol.Add(usuarioRol);
                await _conexionContext.SaveChangesAsync();

                return Ok(new { mensaje = "Usuario y rol asignado exitosamente." });
            }

            return BadRequest("Rol 'User' no encontrado.");
        }

        // Obtener un usuario por ID
        [HttpGet("Usuario/{id}")]
        public async Task<ActionResult<UsuarioModel>> GetUsuarioPorId(int id)
        {
            var usuario = await _conexionContext.usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado." });
            }

            return Ok(usuario);
        }

        // Eliminar un usuario
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> EliminarUsuario(int id)
        {
            var usuario = await _conexionContext.usuario.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado." });
            }

            _conexionContext.usuario.Remove(usuario);
            await _conexionContext.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario eliminado exitosamente." });
        }

        // Login de Usuario
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.UserName) || string.IsNullOrWhiteSpace(login.Password))
            {
                return BadRequest(new { mensaje = "El nombre de usuario y la contraseña son requeridos." });
            }

            var usuario = await _conexionContext.usuario.FirstOrDefaultAsync(u => u.UserName == login.UserName);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Password, usuario.Pass))
            {
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos." });
            }

            var datosUsuario = new
            {
                usuario.Id,
                usuario.UserName,
                usuario.Nombre
            };

            return Ok(new { mensaje = "Login exitoso", usuario = datosUsuario });
        }
    }
}
