using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Aibu.InternshipAutomation.Data.Entities
{
    public class CompanyUserss
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public string? CompanyName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool? IsActive { get; set; }
        public int? RoleId { get; set; }
        public string? EmployeeName { get; set; }
        public string? Title { get; set; }
        public string? MissionArea { get; set; }
        public string? Telephone { get; set; }
        public string? Adress { get; set; }
        public string? ActivityArea { get; set; }
        public string? TotalNumberOfEmployees { get; set; }
        public bool? AllDayWorkingOnWeekends { get; set; }
        public string? TelephoneCompany { get; set; }
        public string? Fax { get; set; }
        public string? CompanyEmail { get; set; }
        public bool? IsComputerEngineer { get; set; }
        public bool? IsElectricalElectronicsEngineering { get; set; }
        public bool? IsMechanicalEngineering { get; set; }
        public bool? IsCivilEngineering { get; set; }
        public bool? IsFoodEngineering { get; set; }
        public bool? IsEnvironmentalEngineering { get; set; }
        public bool? IsChemicalEngineering { get; set; }
        public string? ExistingEngineers { get; set; }
        public Roles Role { get; set; }

    }
}
