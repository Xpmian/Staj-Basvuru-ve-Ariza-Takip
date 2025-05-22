namespace Aibu.InternshipAutomation.Data.Entities
{
    public class AuthorizedDepartments
    {
        public int AuthorizedPersonId { get; set; }
        public AuthorizedPersons AuthorizedPerson { get; set; }

        public int DepartmentId { get; set; }
        public Departments Department { get; set; }

        public int RoleId { get; set; }
        public Roles Role { get; set;}
    }
}
