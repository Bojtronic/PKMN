using Org.BouncyCastle.Math;

namespace Api_Pdx_Db_V2.Models.PokemonModel
{
    public class PokemonModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int height { get; set; }
        public int weight { get; set; }

        public Sprites sprites { get; set; }
        public List<Stat> stats { get; set; }
        public List<Type> types { get; set; }
    }
}
