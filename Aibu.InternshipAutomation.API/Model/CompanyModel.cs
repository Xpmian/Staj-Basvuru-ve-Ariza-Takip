namespace Aibu.InternshipAutomation.API.Model
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Adress { get; set; }
        public string ActivityArea { get; set; }
        public int TotalNumberOfEmployees { get; set; }
        public bool AllDayWorkingOnWeekends { get; set; }
        public string TelephoneCompany { get; set; }
        public string Fax { get; set; }
        public string EmployeerName { get; set; }
        public string Telephone { get; set; }
        public string MissionArea { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
    }
}
