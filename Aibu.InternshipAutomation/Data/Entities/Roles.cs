namespace Aibu.InternshipAutomation.Data.Entities
{
    public class Roles
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Userss> Userss { get; set; }
        public ICollection<States> States { get; set; }
        public ICollection<CompanyUserss> CompanyUsers { get; set; }


    }
}
