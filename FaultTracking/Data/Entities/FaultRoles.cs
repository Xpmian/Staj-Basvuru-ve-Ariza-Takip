namespace FaultTracking.Data.Entities
{
    public class FaultRoles
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public ICollection<AuthorizedPersons> AuthorizedPersons { get; set; }
    }
}
