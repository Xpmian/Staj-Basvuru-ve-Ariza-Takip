using FaultTracking.Data.Base;
using FaultTracking.Data.Context;
using FaultTracking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.Net.Mail;

namespace FaultTracking.Data.Dal
{
    public class FormDal : IFormDal
    {
        private readonly DatabaseContext _databaseContext;

        public FormDal(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public Forms? GetFormtById(int id)
        {
            return _databaseContext.Form.FirstOrDefault(p => p.Id == id);
        }
        public Forms? Add(Forms forms)
        {
            var entity = _databaseContext.Form.Add(forms);
            _databaseContext.SaveChanges();
            return entity.Entity;
        }
        public Forms? Delete(int id)
        {
            var form = GetFormtById(id);
            if (form is null)
            {
                throw new InvalidOperationException("Form bulunamadi");
            }
            var entity = _databaseContext.Form.Remove(form);
            return entity.Entity;
        }
        
        public List<FormStatusViews> GetAll()
        {
            return _databaseContext.FormStatusView.ToList();
        }

        public List<FormStatusViews> CompletedFault()
        {
            var completedFaultList = _databaseContext.FormStatusView.Where(p => p.FormStatusId == 3).ToList();
            return completedFaultList;
        }

        public List<FormStatusViews> WaitingFault()
        {
            var waitingFaultList = _databaseContext.FormStatusView.Where(p => p.FormStatusId == 1).ToList();
            return waitingFaultList;
        }

        public List<FormStatusViews> AppointedFault()
        {
            var appointedFaultList = _databaseContext.FormStatusView.Where(p => p.FormStatusId == 2).ToList();
            return appointedFaultList;
        }
        public Forms? UpdateApprovalStatus(int id, int employeeId , int colourId)
        {
            var existingForm = _databaseContext.Form.FirstOrDefault(p => p.Id == id);
            if (existingForm is null)
                throw new InvalidOperationException("Güncellenemedi");

            existingForm.FormStatusId = existingForm.FormStatusId + 1;
            existingForm.AuthorizedPersonId = employeeId;
            existingForm.ColourId = colourId;

            if(existingForm.FormStatusId > 3)
            {
                throw new InvalidOperationException("Güncellenemedi");
            }

            _databaseContext.Form.Update(existingForm);
            try
            {
                _databaseContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return existingForm;
            }    
            return existingForm;
        }

        public Forms? UpdateApprovalAttendantStatus(int id)
        {
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("dd/MM/yyyy");

            var existingForm = _databaseContext.Form.FirstOrDefault(p => p.Id == id);
            if (existingForm is null)
                throw new InvalidOperationException("Güncellenemedi");

            existingForm.FormStatusId = existingForm.FormStatusId + 1;
            existingForm.CompletionDate = formattedDate;
            if (existingForm.FormStatusId > 3)
            {
                throw new InvalidOperationException("Güncellenemedi");
            }
            _databaseContext.Form.Update(existingForm);
            _databaseContext.SaveChanges();
            return existingForm;
        }

        public List<FaultTypes> GetAllFaultTypes()
        {
            return _databaseContext.FaultType.ToList();
        }

        public List<Colours> GetAllColourCode()
        {
            return _databaseContext.Colour.ToList();
        }

        public List<AuthorizedPersons> GetAuthorizedPerson()
        {
            return _databaseContext.AuthorizedPerson.Where(p => p.FaultRoleId == 2 ).ToList();
        
        }

        public string GetStudent(string studentNumber)
        {
            var studentName = _databaseContext.Ubys.FirstOrDefault(p => p.Number.Equals(studentNumber)).Name;
            return studentName;
        }

        public List<FormStatusViews> PersonelAppointedFaultList(string personelMail)
        {
            var personelList = _databaseContext.AuthorizedPerson.FirstOrDefault(p => p.Email.Equals(personelMail));
            var personelFaultList = _databaseContext.FormStatusView.Where(p => p.AuthorizedPersonId == personelList.Id && p.FormStatusId == 2 ).ToList();
            return personelFaultList;
        }
        public List<FormStatusViews> PersonelCompaletedFaultList(string personelMail)
        {
            var personelList = _databaseContext.AuthorizedPerson.FirstOrDefault(p => p.Email.Equals(personelMail));
            var personelFaultList = _databaseContext.FormStatusView.Where(p => p.AuthorizedPersonId == personelList.Id && p.FormStatusId == 3).ToList();
            return personelFaultList;
        }

        public List<RoleViews> RoleFaultList(string roleMail)
        {
            var roleFaultList = _databaseContext.RoleView.Where(P => P.Email == roleMail).ToList();
            return roleFaultList;
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

        public List<FormStatusViews> ColourFilter(int colourId)
        {
            var filterList = _databaseContext.FormStatusView.Where(p => p.ColourId == colourId).ToList();
            return filterList;
        }

        public List<FormStatusViews> EmployeeFilter(int employeeId)
        {
            var filterList = _databaseContext.FormStatusView.Where(p => p.AuthorizedPersonId == employeeId).ToList();
            return filterList;
        }

        public List<FormStatusViews> DescriptionFilter(string description)
        {
            var filterList = _databaseContext.FormStatusView.Where(p => p.Description.Contains(description)).ToList();
            return filterList;
        }

        public List<FormStatusViews> TitleFilter(string title)
        {
            var filterList = _databaseContext.FormStatusView.Where(p => p.Title.Contains(title)).ToList();
            return filterList;
        }

        public List<FormStatusViews> FieldFilter(string field)
        {
            var filterList = _databaseContext.FormStatusView.Where(p => p.Field.Contains(field)).ToList();
            return filterList;
        }

        public void UploadInfoAuthorized(IFormFile excelFile)
        {
            if (excelFile != null && excelFile.Length > 0)
            {
                try
                {

                    using (var package = new ExcelPackage(excelFile.OpenReadStream()))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;


                        _databaseContext.AuthorizedUbys.RemoveRange(_databaseContext.AuthorizedUbys);
                        _databaseContext.SaveChanges();


                        for (int row = 3; row <= rowCount; row++)
                        {
                            string nameSurname = worksheet.Cells[row, 3].Value?.ToString();
                            string email = worksheet.Cells[row, 7].Value?.ToString();
                            if (!nameSurname.IsNullOrEmpty() || !email.IsNullOrEmpty())
                            {
                                (string ad, string soyad) = SplitLastWord(nameSurname);

                                var authorizedPerson = new AuthorizedUbyss { Name = ad, Surname = soyad, Email = email };
                                _databaseContext.AuthorizedUbys.Add(authorizedPerson);
                            }

                        }
                        _databaseContext.SaveChanges();

                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Excel dosyasi islenirken hata olustu.");
                }
            }
        }

        public void UploadInfoStudent(IFormFile excelFile)
        {
            if (excelFile != null && excelFile.Length > 0)
            {
                try
                {

                    using (var package = new ExcelPackage(excelFile.OpenReadStream()))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;

                        _databaseContext.Ubys.RemoveRange(_databaseContext.Ubys);
                        _databaseContext.SaveChanges();


                        for (int row = 2; row <= rowCount; row++)
                        {
                            string studentNo = worksheet.Cells[row, 1].Value?.ToString();
                            string name = worksheet.Cells[row, 2].Value?.ToString();
                            string surname = worksheet.Cells[row, 3].Value?.ToString();

                            var student = new Ubyss { Number = studentNo, Name = name, Surname = surname };
                            _databaseContext.Ubys.Add(student);
                        }


                        _databaseContext.SaveChanges();

                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Excel dosyasi islenirken hata olustu.");
                }
            }
        }

        public static (string remainingText, string lastWord) SplitLastWord(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return (string.Empty, string.Empty);
            }


            string[] words = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 1)
            {

                return (string.Empty, words[0]);
            }

            string lastWord = words[words.Length - 1];

            string remainingText = string.Join(" ", words, 0, words.Length - 1);

            return (remainingText, lastWord);
        }

        public List<FormStatusViews>? ContinueprocessOwner(string userMail)
        {
            var entity = _databaseContext.FormStatusView.Where(p => p.UserMail == userMail && p.FormStatusId == 1 || p.UserMail == userMail && p.FormStatusId == 2).ToList();      
            return entity;
        }   

        public List<FormStatusViews>? CompletedprocessOwner(string userMail)
        {
            var entity = _databaseContext.FormStatusView.Where(p => p.FormStatusId == 3 && p.UserMail == userMail).ToList();
            return entity;
        }   
    }
}
