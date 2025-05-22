namespace Aibu.InternshipAutomation.Data.Entities
{
    public class States
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public Roles Role { get; set; }
        public ICollection<Students> Students { get; set; }

    }
}
