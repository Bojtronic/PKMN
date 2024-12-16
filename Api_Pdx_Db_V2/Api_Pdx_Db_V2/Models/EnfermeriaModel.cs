namespace Api_Pdx_Db_V2.Models
{
    public class EnfermeriaModel
    {
        public int Id { get; set; }
        public int Idusuario { get; set; }
        public int IdUsuPkm { get; set; }
        public string Descripcion {  get; set; }
        public DateTime fecha { get; set; }
    }
}
