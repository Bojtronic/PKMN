using System.ComponentModel.DataAnnotations;

namespace Api_Pdx_Db_V2.Models
{
    public class UsuarioRolModel
    {
        [Key]
        public int Id { get; set; }

        public int IdRol { get; set; }

        public int IdUsuario { get; set; }
    }
}
