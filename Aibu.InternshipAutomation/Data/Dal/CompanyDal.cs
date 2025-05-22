using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Context;
using Aibu.InternshipAutomation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Components.Authorization;
using Argon;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace Aibu.InternshipAutomation.Data.Dal
{
    public class CompanyDal : ICompanyDal
    {
        private readonly DatabaseContext _databaseContext;

        public CompanyDal(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public List<CompanyUserss> GetAll()
        {
            return _databaseContext.CompanyUsers.ToList();
        }
        public List<AuthorizedViews> GetApplicantStudentByCompanyEmail(string companyName)
        {
            var studentList = _databaseContext.AuthorizedView.Where(p => p.CompanyEmail.Contains(companyName) && p.AcceptanceStatus == 1 && p.RoleId == 5).ToList();
            return studentList;
        }
        public CompanyUserss? CompanyAdd(CompanyUserss companies)
        {
            if (CheckCompanyExist1(companies.CompanyEmail))
            {
                throw new InvalidOperationException("Staja daha once kayit olundu");
            }
            var entity = _databaseContext.CompanyUsers.Add(companies);
            _databaseContext.SaveChanges();
            return entity.Entity;
        }

        private bool CheckCompanyExist(string email)
        {
            return _databaseContext.CompanyUsers.FirstOrDefault(p => p.Email == email) != null;
        }

        private bool CheckCompanyExist1(string email)
        {
            return _databaseContext.CompanyUsers.FirstOrDefault(p => p.CompanyEmail == email) != null;
        }

        public CompanyUserss? Add(CompanyUserss companyUsers)
        {
            if (CheckCompanyExist(companyUsers.Email))
                throw new InvalidOperationException("Bu mail adresi ile hesap oluşturuldu");

            var entity = _databaseContext.CompanyUsers.Add(companyUsers);
            _databaseContext.SaveChanges();
            return entity.Entity;
        }

        public CompanyUserss? Update(CompanyUserss companies)
        {
            var entity = _databaseContext.CompanyUsers.Update(companies);
            return entity.Entity;
        }

        public CompanyUserss? CompanyUserUpdate(CompanyUserss companyUserss,int id)
        {
            if (CheckCompanyExist(companyUserss.Email))
            {
                throw new InvalidOperationException("Bu mail adresi ile hesap oluşturuldu");
            }
                
            var existingCompany = _databaseContext.CompanyUsers.FirstOrDefault(p => p.Id == id);

            if (existingCompany is null)
            {
                throw new InvalidOperationException("Durumu güncellenemedi");
            }

            existingCompany.EmployeeName = companyUserss.EmployeeName;
            existingCompany.Title = companyUserss.Title;
            existingCompany.MissionArea = companyUserss.MissionArea;
            existingCompany.Telephone = companyUserss.Telephone;
            existingCompany.Adress = companyUserss.Adress;
            existingCompany.ActivityArea = companyUserss.ActivityArea;
            existingCompany.TotalNumberOfEmployees = companyUserss.TotalNumberOfEmployees;
            existingCompany.AllDayWorkingOnWeekends = companyUserss.AllDayWorkingOnWeekends;
            existingCompany.TelephoneCompany = companyUserss.TelephoneCompany;
            existingCompany.Fax = companyUserss.Fax;
            existingCompany.CompanyEmail = companyUserss.CompanyEmail;
            existingCompany.IsComputerEngineer = companyUserss.IsComputerEngineer;
            existingCompany.IsElectricalElectronicsEngineering = companyUserss.IsElectricalElectronicsEngineering;
            existingCompany.IsMechanicalEngineering = companyUserss.IsMechanicalEngineering;
            existingCompany.IsCivilEngineering = companyUserss.IsCivilEngineering;
            existingCompany.IsFoodEngineering = companyUserss.IsFoodEngineering;
            existingCompany.IsEnvironmentalEngineering = companyUserss.IsEnvironmentalEngineering;
            existingCompany.IsChemicalEngineering = companyUserss.IsChemicalEngineering;
            existingCompany.ExistingEngineers = companyUserss.ExistingEngineers;

            _databaseContext.CompanyUsers.Update(existingCompany);
            _databaseContext.SaveChanges();
            return existingCompany;
        }

        public CompanyUserss? GetCompanyById(int id)
        {
            return _databaseContext.CompanyUsers.FirstOrDefault(p => p.Id == id);
        }

        public CompanyUserss? GetCompanyIdByEmail(string email)
        {
            return _databaseContext.CompanyUsers.FirstOrDefault(p => p.Email.Contains(email));
        }

        public CompanyUserss? Delete(int id)
        {
            var company = GetCompanyById(id);
            if (company is null)
                throw new InvalidOperationException("Şirket bilgisi bulunamadi");

            var entity = _databaseContext.CompanyUsers.Remove(company);
            return entity.Entity;
        }

        public void AddMail(string userEmail, string companyName)
        {
            var companyUsers = _databaseContext.CompanyUsers.FirstOrDefault(x => x.Email == userEmail);
            if (companyUsers is not null)
                throw new InvalidDataException("Şirket daha önce oluşturulmuş");
            var newCompany = new CompanyUserss
            {
                CompanyName = companyName,
                Email = userEmail
            };

            _databaseContext.CompanyUsers.Add(newCompany);
            _databaseContext.SaveChanges();
        }

        public void UpdateResetToken(string userEmail, string token)
        {
            var user = _databaseContext.CompanyUsers.FirstOrDefault(u => u.Email == userEmail);
            if (user is null)
                throw new InvalidOperationException("Şirket bilgisi bulunamadı");
            user.Token = token;
            _databaseContext.SaveChanges();

        }

        public CompanyUserss? GetUserByResetToken(string token)
        {
            return _databaseContext.CompanyUsers.FirstOrDefault(u => u.Token == token);
        }

        public void UpdateUser(string tokenuser, string password)
        {
            var user = GetUserByResetToken(tokenuser.ToString());
            if (user is null)
                throw new InvalidOperationException("Token bulunamadı");
            user.Password = HashPassword(password);
            user.Token = null;
            user.CreateTime = DateTime.Now;
            user.UpdateTime = DateTime.Now;
            user.IsActive = true;
            user.RoleId = 4;
            _databaseContext.CompanyUsers.Update(user);
            _databaseContext.SaveChanges();
        }

        public string HashPassword(string password)
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] dizi = Encoding.UTF8.GetBytes(password);
            dizi = md5.ComputeHash(dizi);
            StringBuilder sb = new StringBuilder();
            foreach (byte ba in dizi)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }
            return sb.ToString();
        }

        //public string HashPassword(string password)
        //{
        //    using (var sha256 = SHA256.Create())
        //    {
        //        byte[] dizi = Encoding.UTF8.GetBytes(password);
        //        dizi = sha256.ComputeHash(dizi);
        //        StringBuilder sb = new StringBuilder();
        //        foreach (byte ba in dizi)
        //        {
        //            sb.Append(ba.ToString("x2").ToLower());
        //        }
        //        return sb.ToString();
        //    }
        //}

        public async Task SendMailAsync(string userEmail, string token, string mailBody,string subject)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(userEmail);
            mailMessage.Subject = subject;
            mailMessage.Body = mailBody;
            mailMessage.From = new MailAddress("onlinemuhendislik14@gmail.com");

            using var smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            //smtpClient.Credentials = new System.Net.NetworkCredential("sirketdeneme6@gmail.com", "yztl hfjn bbeq jtpg");
            
            smtpClient.Credentials = new System.Net.NetworkCredential("onlinemuhendislik14@gmail.com", "rhbt hrcx grzu dwvh");

            smtpClient.EnableSsl = true;
            await smtpClient.SendMailAsync(mailMessage);
        }

        public bool CheckCompanyUserExistPassword(string companyEmail)
        {
            var company = _databaseContext.CompanyUsers.Where(p => p.Email == companyEmail && !string.IsNullOrEmpty(p.Password));
            if (!company.Any())
            {
                return false;
            }
            return true;
        }
            
        public bool CheckCompanyUserExist(string companyEmail)
        {
            var company = _databaseContext.CompanyUsers.Where(p => p.Email == companyEmail);
            if (!company.Any())
            {
                return false;
            }
            return true;
        }         

        public string GetToken(string companyEmail)
        {
            var token = _databaseContext.CompanyUsers.FirstOrDefault(p => p.Email == companyEmail);

            return token.Token;
        }
        public Students UpdateAcceptState(int id, int stateId)
        {
            var existingStudent = _databaseContext.Student.FirstOrDefault(p => p.Id == id);

            if (existingStudent is null)
            {
                throw new InvalidOperationException("Durumu güncellenemedi");
            }

            existingStudent.StateId = existingStudent.StateId + 1;
            existingStudent.UpdateTime = DateTime.Now;

            if (stateId == 3)
            {
                existingStudent.SirketUpdateTime = DateTime.Now;
                existingStudent.StajKomisyonUpdateTime = DateTime.Now;
                existingStudent.FakulteSekreteriUpdateTime = DateTime.Now;
                existingStudent.BolumSekreteriUpdateTime = DateTime.Now;
            }

            _databaseContext.Student.Update(existingStudent);
            _databaseContext.SaveChanges();
            return existingStudent;
        }


        public bool CheckCompany(string email)
        {
            var companyList = _databaseContext.CompanyUsers.FirstOrDefault(p => p.Email.Contains(email));

            if(companyList.EmployeeName == null && companyList.Title == null && companyList.MissionArea == null && companyList.Telephone == null)
            {
                return false;
            }

            return true;
        }

        public List<CompanyInfoViews> GetCompanyInfoViews ()
        {
            var infoList = _databaseContext.CompanyInfoView.ToList();

            return infoList;
        }

        public List<CompanyUserss>? CompanyIdByEmail1(string email)
        {

            return _databaseContext.CompanyUsers.Where(p => p.Email.Equals(email)).ToList();
        }

        public List<AuthorizedViews> CompanyApproved(string email)
        {
            List<AuthorizedViews> studentList = new List<AuthorizedViews>();
            studentList = _databaseContext.AuthorizedView.Where(p => p.CompanyEmail == email && p.RoleId == 9).ToList();

            return studentList;
        }

        //public List<Students> GetDepartment(string email)
        //{
                
        //}
    }
}
