namespace Aibu.InternshipAutomation.Data.Entities
{
    public class Ubyss
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Class { get; set; }

        public Departments Department { get; set; }




    }
}
