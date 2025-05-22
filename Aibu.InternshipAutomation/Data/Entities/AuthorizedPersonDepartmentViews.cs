namespace Aibu.InternshipAutomation.Data.Entities
{
    public class AuthorizedPersonDepartmentViews
    {
        public int AuthorizedPersonId { get; set; }
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}
