namespace FaultTracking.Data.Entities
{
    public class AuthorizedPersons
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int FaultRoleId { get; set; }
        public FaultRoles FaultRole { get; set; }
        public ICollection<Forms> Forms { get; set; }
    }
}
