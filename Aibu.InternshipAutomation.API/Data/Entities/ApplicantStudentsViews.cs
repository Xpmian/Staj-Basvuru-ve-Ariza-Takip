namespace Aibu.InternshipAutomation.Data.Entities
{
    public class ApplicantStudentsViews
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TypeOfInternship { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string CompanyName { get; set; }
        public int AcceptanceStatus { get; set; }
    }
}
