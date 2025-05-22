namespace Aibu.InternshipAutomation.Data.Entities
{
    public class InternPeriods
    {
        public int Id { get; set; }
        public string TypeOfInternship { get; set; }
        public ICollection<Students> Students { get; set; }
    }
}
