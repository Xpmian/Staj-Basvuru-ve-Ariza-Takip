using Aibu.InternshipAutomation.Data.Entities;

namespace Aibu.InternshipAutomation.Data.Base
{
    public interface ICompanyDal
    {
        public List<Companies> GetAll();
        public Companies? Add(Companies companies);
        public Companies? Update(Companies companies);
        public Companies? Delete(int id);
        public List<ApplicantStudentsViews> GetApplicantStudentByCompanyName(string companyName);
        public Companies? GetCompanyById(int id);
    }
}
