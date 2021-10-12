namespace PokedexAPI_V3.Models
{
    public class Move
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Power { get; set; }
        public string SpecialEffects { get; set; }
    }
}