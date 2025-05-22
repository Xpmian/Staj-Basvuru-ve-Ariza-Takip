using Aibu.InternshipAutomation.Data.Entities;
using Microsoft.AspNetCore.Components.Authorization;

namespace Aibu.InternshipAutomation.Data.Base
{
    public interface ICompanyDal
    {
        public List<CompanyUserss> GetAll();
        //public Companies? Add(Companies companies);
        public CompanyUserss? Update(CompanyUserss companies);
        public CompanyUserss? Delete(int id);
        public List<AuthorizedViews> GetApplicantStudentByCompanyEmail(string companyName);
        public CompanyUserss? GetCompanyById(int id);
        public void UpdateResetToken(string userEmail, string token);
        public void AddMail(string userEmail, string companyName);
        public CompanyUserss? GetUserByResetToken(string token);
        public void UpdateUser(string tokenuser, string password);
        public Students UpdateAcceptState(int id, int stateId);
        public string HashPassword(string password);
        public async Task SendMailAsync(string userEmail, string token, string mailBody, string subject) { }
        public bool CheckCompanyUserExist(string companyEmail);

        public bool CheckCompanyUserExistPassword(string companyEmail);
        public string GetToken(string companyEmail);
        public CompanyUserss? GetCompanyIdByEmail(string email);
        public CompanyUserss? CompanyAdd(CompanyUserss companies);
        public CompanyUserss? CompanyUserUpdate(CompanyUserss companyUserss, int id);
        public bool CheckCompany(string email);
        public List<CompanyInfoViews> GetCompanyInfoViews();
        public List<CompanyUserss>? CompanyIdByEmail1(string email);
        public List<AuthorizedViews> CompanyApproved(string email);
    }
}
