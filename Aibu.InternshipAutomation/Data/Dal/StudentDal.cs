using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Context;
using Aibu.InternshipAutomation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Net.Mail;

namespace Aibu.InternshipAutomation.Data.Dal
{
    public class StudentDal : IStudentDal
    {

        private readonly DatabaseContext _databaseContext;

        public StudentDal(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public List<Students> GetAll()
        {
            return _databaseContext.Student.ToList();

        }

        private bool CheckInternshipExist(string number, int internType , int acceptStatus)
        {
            return _databaseContext.Student.FirstOrDefault(student => string.Equals(number, student.Number) && student.InternTypesId == internType && student.AcceptanceStatusId == acceptStatus) != null;
        }

        public Students? GetStudentById(int id)
        {
            return _databaseContext.Student.FirstOrDefault(student => student.Id == id);
        }
        public Students? Add(Students student)
        {
            if (CheckInternshipExist(student.Number, student.InternTypesId , student.AcceptanceStatusId))
            {
                throw new InvalidOperationException("Staja daha once kayit olundu");
            }

            var entity = _databaseContext.Student.Add(student);
            _databaseContext.SaveChanges();
            return entity.Entity;
        }
        public Students? Delete(int id)
        {
            var student = GetStudentById(id);
            if (student is null)
            {
                throw new InvalidOperationException("Staj verisi bulunamadi");
            }
            var entity = _databaseContext.Student.Remove(student);
            return entity.Entity;
        }


        public Students? UpdateAcceptanceStatus(Students student)
        {
            var existingStudent = _databaseContext.Student.Find(student.Id);
            if (existingStudent is null)
                throw new InvalidOperationException("Kabul Durumu güncellenemedi");

            existingStudent.AcceptanceStatus = student.AcceptanceStatus;
            _databaseContext.Student.Update(existingStudent);
            _databaseContext.SaveChanges();

            return existingStudent;
        }

        public List<PastInternShipViews> GetAllView()
        {
            return _databaseContext.PastInternShipView.ToList();
        }
        public int DateComparison(DateTime InternShipEndDate)
        {
            DateTime utcDateTime = DateTime.UtcNow;
            // InternShipEndDate, utcDateTime'den önceyse sonuç negatif
            // InternShipEndDate ve utcDateTime aynıysa sonuç 0
            // InternShipEndDate, utcDateTime'den sonraysa sonuç pozitif
            int result = InternShipEndDate.CompareTo(utcDateTime);
            return result;
        }
        public List<PastInternShipViews> PastInternShip(string number)
        {
            var studentList = _databaseContext.PastInternShipView.Where(p => p.Number.Contains(number));
            List<PastInternShipViews> pastInternShipList = new List<PastInternShipViews>();

            foreach (var item in studentList)
            {
                int compareValue = DateComparison(item.IntershipEndDate);
                int acceptanceStatus = item.AcceptanceStatus;
                if (compareValue < 0 && acceptanceStatus == 1)
                {
                    pastInternShipList.Add(item);
                }
            }
            return pastInternShipList;
        }
        public List<Students> InformationByStudentNumber(string number)
        {
            var studentList = _databaseContext.Student.Where(p => p.Number.Contains(number)).ToList();
            return studentList;
        }

        public string GetStudentByName(string name)
        {
            var studentList = _databaseContext.PastInternShipView.FirstOrDefault(p => p.Name.Contains(name));
            return studentList.Number;
        }

        public List<StateViews> GetAllState(string studentNumber)
        {
            var stateList = _databaseContext.StateView.Where(p => p.Number.Contains(studentNumber) && p.AcceptanceStatusId == 1).ToList();
            return stateList;
        }
      
        public Students UpdateAcceptState(int id, int stateId)
        {
            var existingStudent = _databaseContext.Student.FirstOrDefault(p => p.Id == id);

            if (existingStudent is null)
            {
                throw new InvalidOperationException("Durumu güncellenemedi");
            }

            if (stateId == 2)
            {
                existingStudent.BolumBaskanıUpdateTime = DateTime.Now;
                existingStudent.SirketUpdateTime = DateTime.Now;
                existingStudent.StajKomisyonUpdateTime = DateTime.Now;
                existingStudent.FakulteSekreteriUpdateTime = DateTime.Now;
                existingStudent.BolumSekreteriUpdateTime = DateTime.Now;
            }
            else if (stateId == 4)
            {
                existingStudent.StajKomisyonUpdateTime = DateTime.Now;
                existingStudent.FakulteSekreteriUpdateTime = DateTime.Now;
                existingStudent.BolumSekreteriUpdateTime = DateTime.Now;
            }
            else if (stateId == 5)
            {
                existingStudent.FakulteSekreteriUpdateTime = DateTime.Now;
                existingStudent.BolumSekreteriUpdateTime = DateTime.Now;
            }
            else if (stateId == 6)
            {
                existingStudent.BolumSekreteriUpdateTime = DateTime.Now;
            }
            existingStudent.StateId = existingStudent.StateId + 1;
            existingStudent.UpdateTime = DateTime.Now;

            _databaseContext.Student.Update(existingStudent);
            _databaseContext.SaveChanges();

            if(stateId == 4)
            {
                var email = _databaseContext.AuthorizedPersonDepartmentView.Where(p => p.RoleId == 7 && p.DepartmentId == existingStudent.DepartmentId).FirstOrDefault().Email;
                string ogrenciAdi = existingStudent.Name + " " + existingStudent.Surname;
                DateTime basvuruTarihi = (DateTime)existingStudent.StajKomisyonUpdateTime;
                string basvuruTarihiString = existingStudent.StajKomisyonUpdateTime?.ToString("dd.MM.yyyy");
                var subject = "Staj Başvurusu - " + ogrenciAdi;
                var mailUpdate = $"Sayın Fakülte Sekreteri,\n" +
                             $"Bu e-posta, {ogrenciAdi} adlı öğrencinin staja başvurduğunu bildirmek için gönderilmektedir.\n" +
                             $"Staj Komisyonu Onaylanma Tarihi: {basvuruTarihiString}\n" +
                             $"Gerekli incelemelerin ve işlemlerin yapılmasını rica ederim.\n" +
                             $"Saygılarımla";
                SendMailAsync(email,mailUpdate, subject);
            }
            return existingStudent;
        }

        public Students? UpdateRejectState(int id, string description)
        {
            var existingStudent = _databaseContext.Student.FirstOrDefault(p => p.Id == id);
            if (existingStudent is null)
            {
                throw new InvalidOperationException("Durumu güncellenemedi");
            }

            existingStudent.AcceptanceStatusId = 2;
            existingStudent.Description = description;
            existingStudent.UpdateTime = DateTime.Now;
            _databaseContext.Student.Update(existingStudent);

            try
            {
                _databaseContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return existingStudent;
            }
            return existingStudent;
        }

        public Students? RejectStudent(int id)
        {
            var existingStudent = _databaseContext.Student.FirstOrDefault(p => p.Id == id);
            if (existingStudent is null)
            {
                throw new InvalidOperationException("Durumu güncellenemedi");
            }

            existingStudent.AcceptanceStatusId = 2;
            existingStudent.UpdateTime = DateTime.Now;

            _databaseContext.Student.Update(existingStudent);

            try
            {
                _databaseContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return existingStudent;
            }
            return existingStudent;
        }

        public async Task SendMailAsync(string userEmail, string mailBody, string subject)
            {
                var mailMessage = new MailMessage();
                mailMessage.To.Add(userEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = mailBody;
                mailMessage.From = new MailAddress("onlinemuhendislik14@gmail.com");

                using var smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential("onlinemuhendislik14@gmail.com", "rhbt hrcx grzu dwvh");
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);
            }
        public List<Students> GetAll(string number)
        {
            var studentList = _databaseContext.Student.Where(p => p.Number.Contains(number)).ToList();
            return studentList;
        }
        public Boolean GetStatus(string number)
        {
            var studentStatus = _databaseContext.Student.FirstOrDefault(p => p.Number.Contains(number));
            if(studentStatus.StateId == 7 && studentStatus.AcceptanceStatusId == 1)
            {
                return true;
            }
            return false;
        }

        public List<AuthorizedViews> GetStudentForPdf(string number)
        {
            var studentList = _databaseContext.AuthorizedView.Where(p => p.Number.Contains(number) && p.AcceptanceStatus == 1 && p.RoleId == 9).ToList();
            return studentList;
        }

        public List<AuthorizedViews> GetStudentContinuing(string number)
        {
            var studentList = _databaseContext.AuthorizedView.Where(p => p.Number.Contains(number) && p.AcceptanceStatus == 1 && p.RoleId != 9).ToList();
            return studentList;
        }

        public string ExtractStudentNumber(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            int atIndex = email.IndexOf('@');
            if (atIndex == -1)
            {
                throw new ArgumentException("Invalid email format", nameof(email));
            }

            return email.Substring(0, atIndex);
        }

        public bool InternType1(string studentNumber)
        {
            var internTypes1 = _databaseContext.Student.Where(p => p.Number.Contains(studentNumber) && p.InternTypesId == 1 && p.AcceptanceStatusId == 1).ToList();

            if (internTypes1.Count == 0)
            {
                return true;
            }
            return false;
        }

        public bool InternType2(string studentNumber)
        {
            var internTypes2 = _databaseContext.Student.Where(p => p.Number.Contains(studentNumber) && p.InternTypesId == 2 && p.AcceptanceStatusId == 1).ToList();

            if (internTypes2.Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}

