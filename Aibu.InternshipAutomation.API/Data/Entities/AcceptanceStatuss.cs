namespace Aibu.InternshipAutomation.Data.Entities
{
    public class AcceptanceStatuss
    {
        public int Id { get; set; }
        public int AcceptanceStatus { get; set; }
        public ICollection<Students> Students { get; set; }
    }
}