namespace FaultTracking.Data.Entities
{
    public class Colours
    {
        public int Id { get; set; }
        public string ColourCode { get; set; }
        public string ColorMeaning { get; set; } 
        public ICollection<Forms> Forms { get; set; }
    }
}
