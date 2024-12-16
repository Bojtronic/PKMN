namespace Api_Pdx_Db_V2.Models.PokemonModel
{
    public class Stat
    {
        public int base_stat { get; set; }
        public int effort { get; set; }
        public Stat2 stat { get; set; }
    }
}
