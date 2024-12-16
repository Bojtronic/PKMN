namespace PoyectoPokedexApi.Models.UsuarioModel
{
    public class PkmDebilitados
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int pkm_id { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }
        public string NombreUsuario { get; set; }
    }
}
