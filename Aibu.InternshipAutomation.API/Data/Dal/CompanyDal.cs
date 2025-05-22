using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Context;
using Aibu.InternshipAutomation.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aibu.InternshipAutomation.Data.Dal
{
    public class CompanyDal : ICompanyDal
    {
        private readonly DatabaseContext _databaseContext;

        public CompanyDal(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public List<Companies> GetAll()
        {
            return _databaseContext.Company.ToList();
        }
        public List<ApplicantStudentsViews> GetApplicantStudentByCompanyName(string companyName)
        {
            var a = _databaseContext.ApplicantStudentsView.Where(p => p.CompanyName.Contains(companyName));
            List<ApplicantStudentsViews> list = new List<ApplicantStudentsViews>();

            foreach (var item in a)
            {
                int temp = item.AcceptanceStatus;
                if (temp == 1)
                {
                    list.Add(item);
                }
            }
            return list;
        }
        private bool CheckCompanyExist(string email)
        {
            return _databaseContext.Company.FirstOrDefault(p => p.Email == email) != null;
        }
        public Companies? Add(Companies companies)
        {
            if (CheckCompanyExist(companies.Email))
            {
                throw new InvalidOperationException("Bu mail adresi ile hesap oluşturuldu");
            }
            var entity = _databaseContext.Company.Add(companies);
            return entity.Entity;
        }

        public Companies? Update(Companies companies)
        {
            var entity = _databaseContext.Company.Update(companies);
            return entity.Entity;
        }

        public Companies? GetCompanyById(int id)
        {
            return _databaseContext.Company.FirstOrDefault(p => p.Id == id);
        }

        public Companies? Delete(int id)
        {
            try
            {
                var company = GetCompanyById(id);
                if (company is null)
                {
                    throw new InvalidOperationException("Şirket bilgisi bulunamadi");
                }                   
                var entity = _databaseContext.Company.Remove(company);
                return entity.Entity;
            }
            catch (Exception)
            {
                return null;
            }
        }     
    }
}
