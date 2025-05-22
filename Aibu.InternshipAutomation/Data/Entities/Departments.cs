namespace Aibu.InternshipAutomation.Data.Entities
{
    public class Departments
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Students> Students { get; set; }
        public ICollection<Userss> Userss { get; set; }

        public ICollection<Ubyss>  Ubyss { get; set; }

        public ICollection<AuthorizedDepartments> AuthorizedDepartments { get; set; }
    }
}
