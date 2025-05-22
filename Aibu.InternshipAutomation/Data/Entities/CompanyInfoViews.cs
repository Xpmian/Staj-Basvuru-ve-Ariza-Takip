namespace Aibu.InternshipAutomation.Data.Entities
{
    public class CompanyInfoViews
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? EmployeeName { get; set; } = string.Empty;
        public string? Title { get; set; } = string.Empty;
        public string? MissionArea { get; set; } = string.Empty;
        public string? Telephone { get; set; } = string.Empty;
        public string? Adress { get; set; } = string.Empty;
        public string? ActivityArea { get; set; } = string.Empty;
        public string? TotalNumberOfEmployees { get; set; } = string.Empty;
        public bool AllDayWorkingOnWeekends { get; set; }
        public string? TelephoneCompany { get; set; } = string.Empty;
        public string? Fax { get; set; } = string.Empty;
        public string? CompanyEmail { get; set; } = string.Empty;
        public bool? IsComputerEngineer { get; set; }
        public bool? IsElectricalElectronicsEngineering { get; set; }
        public bool? IsMechanicalEngineering { get; set; }
        public bool? IsCivilEngineering { get; set; }
        public bool? IsFoodEngineering { get; set; }
        public bool? IsEnvironmentalEngineering { get; set; }
        public bool? IsChemicalEngineering { get; set; }
        public string? ExistingEngineers { get; set; }
    }
}
