using Aibu.InternshipAutomation.Data.Entities;
using Aibu.InternshipAutomation.Models;

namespace Aibu.InternshipAutomation.Models;

public class CompanyViewModel
{
    public CompanyUsersModel CompanyUser { get; set; }
    public List<CompanyUserss> CompanyInfoView { get; set; }
}
