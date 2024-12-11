using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace Api_Pdx_Db_V2.Controllers
{
    [ApiController]
    [Route("Api_Pdx_DbV2/[controller]")]
    public class ConexionController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public ConexionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult Conectar() 
        {
            string connectionString = _configuration.GetConnectionString("AccesoConexion");
            try
            {
                using (var conexion = new MySqlConnection(connectionString))
                {
                    conexion.Open();
                    return Ok("Conexion Exitosa");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
