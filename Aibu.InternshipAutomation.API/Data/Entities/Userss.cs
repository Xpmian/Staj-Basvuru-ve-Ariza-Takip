using System.Collections;

namespace Aibu.InternshipAutomation.Data.Entities
{
    public class Userss
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsActive { get; set; }
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }
        public Departments Department { get; set; }
        public Roles Role { get; set; }

        

    }
}
