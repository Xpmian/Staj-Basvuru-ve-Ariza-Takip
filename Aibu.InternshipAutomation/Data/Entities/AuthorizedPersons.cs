namespace Aibu.InternshipAutomation.Data.Entities
{
    public class AuthorizedPersons
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public int? DepartmentId { get; set; }
        public int? RoleId { get; set; }
        public Departments Department { get; set; }
        public Roles Role { get; set; }

        public ICollection<AuthorizedDepartments> AuthorizedDepartments { get; set; }
    }
}
