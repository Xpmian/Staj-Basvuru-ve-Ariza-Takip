namespace Aibu.InternshipAutomation.Data.Entities
{
    public class AuthorizedViews
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string TypeOfInternship { get; set; } = string.Empty;
        public int DepartmentId { get; set; } 
        public int AcceptanceStatus { get; set; }
        public int RoleId { get; set; }
        public short Class { get; set; }
        public string IdentificationNumber { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public string CompanyName { get; set;} = string.Empty;
        public string CompanyEmail { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string TelephoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string MotherName { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public DateTime IntershipStartDate { get; set; }
        public DateTime IntershipEndDate { get; set; }
        public string DogumYeri { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime? UpdateTime { get; set; }

    }
}
