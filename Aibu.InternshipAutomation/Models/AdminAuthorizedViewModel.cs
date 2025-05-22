using Aibu.InternshipAutomation.Data.Entities;
using System;

namespace Aibu.InternshipAutomation.Models
{
    public class AdminAuthorizedViewModel
    {
        public List<AuthorizedPersonDepartmentViews> BolumBaskaniList { get; set; }
        public List<AuthorizedPersonDepartmentViews> StajKomisyonList { get; set; }
        public List<AuthorizedPersonDepartmentViews> FakulteSekreteriList { get; set; }
        public List<AuthorizedPersonDepartmentViews> BolumSekreteriList { get; set; }
        public AuthorizedPersonModel AuthorizedPerson { get; set; }

        public int DepartmentId { get; set; }
    }
}
