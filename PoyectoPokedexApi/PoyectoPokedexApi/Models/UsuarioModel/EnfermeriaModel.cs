namespace PoyectoPokedexApi.Models.UsuarioModel
{
    public class EnfermeriaModel
    {
        public int Id { get; set; }
        public int Idusuario { get; set; }
        public int IdUsuPkm { get; set; }
        public string ?Descripcion { get; set; }
        public DateTime fecha { get; set; }
    }
}
