namespace Api_Pdx_Db_V2.Models
{
    public class UsuarioPkmModel
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int pkm_id { get; set; }
        public string nombre { get; set; }
        public int estado { get; set; }

    }
    public class UsuarioPkmConNombreUsuario
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int pkm_id { get; set; }
        public string nombre { get; set; }
        public int estado { get; set; }
        public string NombreUsuario { get; set; } // El nombre del usuario
    }
}
