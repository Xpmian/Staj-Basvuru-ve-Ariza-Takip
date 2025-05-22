namespace Aibu.InternshipAutomation.Data.Entities
{
    public class Students
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public short Class { get; set; }
        public string IdentificationNumber { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string Address { get; set; }
        public string TelephoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
        public int InternType { get; set; }
        public DateTime IntershipStartDate { get; set; }
        public DateTime IntershipEndDate { get; set; }
        public int InternPeriodId { get; set; }
        public int PlaceOfBirthId { get; set; }
        public int DepartmentId { get; set; }
        public int StateId { get; set; }
        public int AcceptanceStatusId { get; set; }
        public States State { get; set; }
        public Departments Department { get; set; }
        public InternPeriods InternPeriod { get; set; }
        public Cities PlaceOfBirth { get; set; }
        public AcceptanceStatuss AcceptanceStatus { get; set; }
    }
}
