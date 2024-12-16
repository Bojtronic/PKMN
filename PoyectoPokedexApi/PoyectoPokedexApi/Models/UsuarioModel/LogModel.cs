using System.ComponentModel.DataAnnotations;

namespace PoyectoPokedexApi.Models.UsuarioModel
{
    public class LogModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
