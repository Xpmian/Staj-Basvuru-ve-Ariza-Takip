namespace Aibu.InternshipAutomation.Data.Entities
{
    public class PastInternShipViews
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public DateTime IntershipStartDate { get; set; }
        public DateTime IntershipEndDate { get; set; }
        public int InternType { get; set; }
        public string TypeOfInternship { get; set; } = string.Empty;
        public int AcceptanceStatus { get; set; }
    }
}
