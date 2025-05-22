namespace Aibu.InternshipAutomation.Data.Entities
{
    public class AuthorizedViews
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string TypeOfInternship { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public int DepartmentId { get; set; } 
        public int AcceptanceStatus { get; set; }
        public int RoleId { get; set; }
    }
}
