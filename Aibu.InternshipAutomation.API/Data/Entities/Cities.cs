namespace Aibu.InternshipAutomation.Data.Entities
{
    public class Cities
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Students> Students { get; set; }
    }
}
