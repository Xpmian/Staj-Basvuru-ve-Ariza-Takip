namespace Aibu.InternshipAutomation.Data.Entities
{
    public class Logs
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string Message { get; set; }
    }
}
