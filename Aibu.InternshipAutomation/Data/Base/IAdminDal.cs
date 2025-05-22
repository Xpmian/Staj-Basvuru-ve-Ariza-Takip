using Aibu.InternshipAutomation.Data.Entities;

namespace Aibu.InternshipAutomation.Data.Base
{
    public interface IAdminDal
    {
        public List<CompanyInfoViews> GetAll();
        public CompanyUserss GetAllInfo(string companyName);
        public AuthorizedPersons? UpdateAuthorizedPerson(int roleId, string name, string surname, string email, int deparmentId);

        public void UploadInfoStudent(IFormFile excelFile);

        public void UploadInfoAuthorized(IFormFile excelFile);
    }
}
