namespace Aibu.InternshipAutomation.Data.Entities
{
    public class InternTypes
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public ICollection<Students> Students { get; set; }
    }
}
