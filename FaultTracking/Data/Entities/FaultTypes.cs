namespace FaultTracking.Data.Entities
{
    public class FaultTypes
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public ICollection<Forms> Forms { get; set; }
    }
}
