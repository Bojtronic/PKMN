﻿using Api_Pdx_Db_V2.Data;
using Api_Pdx_Db_V2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

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

       // este es de prueba apara ver si trae los datos no utilizar en produccion dado

        [HttpGet]
        public ActionResult<IEnumerable<UsuarioModel>> GetUsuario()
        {
            return Ok(_conexionContext.usuario.ToList());
        }

        // Crear un nuevo usuario
        [HttpPost("CrearUsuario")]
        public ActionResult<UsuarioModel> CrearUsuario([FromBody] UsuarioModel nuevoUsuario)
        {
            //Agrega el pass encriptado
            nuevoUsuario.Pass = BCrypt.Net.BCrypt.HashPassword(nuevoUsuario.Pass);
            _conexionContext.usuario.Add(nuevoUsuario);
            _conexionContext.SaveChanges();
            

            //Asigna rol sario 
            var rol = _conexionContext.rol.FirstOrDefault(r => r.Descripcion == "User");

            if (rol != null)
            {
                // Crear la relación en la tabla Usuario_Rol
                var usuarioRol = new UsuarioRolModel
                {
                    IdUsuario = nuevoUsuario.Id, // Asignar el ID del nuevo usuario
                    IdRol = rol.Id // Asignar el ID del rol 'User'
                };

                // Agregar la relación a la tabla Usuario_Rol
                _conexionContext.usuario_rol.Add(usuarioRol);
               

                return Ok("Usuario y rol asignado exitosamente.");
            }
            else
            {
                return BadRequest("Rol 'User' no encontrado.");
            }

          
        } 

        // Obtener un usuario por ID
        [HttpGet("Usuario/{id}")]
        public ActionResult<UsuarioModel> GetUsuarioPorId(int id)
        {
            var usuario = _conexionContext.usuario.Find(id);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            return Ok(usuario);
        }

        // Eliminar un usuario
        [HttpDelete("Eliminar/{id}")]
        public ActionResult EliminarUsuario(int id)
        {
            var usuario = _conexionContext.usuario.FirstOrDefault(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            
            _conexionContext.usuario.Remove(usuario);

            _conexionContext.SaveChanges();

            return Ok("Usuario eliminado exitosamente.");
        }

        // Login de Usuario
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginModel login)
        {
            // Buscar usuario por UserName
            var usuario = _conexionContext.usuario.FirstOrDefault(u => u.UserName == login.UserName);

            if (usuario == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            // Verificar contraseña (hashed)
            if (!BCrypt.Net.BCrypt.Verify(login.Password, usuario.Pass))
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }
            var datosUsuario = new
            {
                usuario.Id,
                usuario.UserName,
                usuario.Nombre
            };

            //retorno usuario 

            return Ok(new { mensaje = "Login exitoso", usuario = datosUsuario });
        }

        [HttpGet("VistaUsuariosRoles")]
        public ActionResult<IEnumerable<UsuarioRolViewModel>> GetVistaUsuariosRoles()
        {
            try
            {
                var data = _conexionContext.VistaUsuariosRoles.ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al consultar la vista: {ex.Message}");
            }
        }

        // Editar un usuario existente
        [HttpPut("EditarUsuario/{id}")]
        public ActionResult EditarUsuario(int id, [FromBody] UsuarioModel usuarioActualizado)
        {
            // Buscar el usuario por ID
            var usuarioExistente = _conexionContext.usuario.FirstOrDefault(u => u.Id == id);

            if (usuarioExistente == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Actualizar los datos del usuario existente
            usuarioExistente.Nombre = usuarioActualizado.Nombre;
            usuarioExistente.UserName = usuarioActualizado.UserName;

            // Si se envía una nueva contraseña, actualizarla
            if (!string.IsNullOrEmpty(usuarioActualizado.Pass))
            {
                usuarioExistente.Pass = BCrypt.Net.BCrypt.HashPassword(usuarioActualizado.Pass);
            }

            // Guardar los cambios en la base de datos
            try
            {
                _conexionContext.SaveChanges();
                return Ok("Usuario actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el usuario: {ex.Message}");
            }
        }


    }
}
