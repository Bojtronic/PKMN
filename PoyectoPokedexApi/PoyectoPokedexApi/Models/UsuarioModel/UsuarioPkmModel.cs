namespace PoyectoPokedexApi.Models.UsuarioModel
{
    public class UsuarioPkmModel
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int pkm_id { get; set; }
        public string nombre { get; set; }
        public int estado { get; set; }
    }
}
