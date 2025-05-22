using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Context;
using Aibu.InternshipAutomation.Data.Entities;
using Aibu.InternshipAutomation.Helper;
using Aibu.InternshipAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using System;
using VerifyTests;

namespace Aibu.InternshipAutomation.Controllers
{
    [Authorize(Roles = "Admin,Staj Komisyonu,Bölüm Başkanı,Fakülte Sekreteri,Bölüm Sekreteri")]
    public class AuthorizedController : Controller
    {
        private readonly IAuthorizedDal _authorizedDal;
        private readonly DatabaseContext _databaseContext;
        private readonly IStudentDal _studentDal;
        private readonly ICompanyDal _companyDal;
        private readonly IPdfHelper _pdfHelper;
        private readonly ILogDal _logDal;
        private readonly IDepartmentDal _departmentDal;
        public AuthorizedController(IAuthorizedDal authorizedDal, IStudentDal studentDal, ICompanyDal companyDal, IPdfHelper helper, DatabaseContext databaseContext, ILogDal logDal, IDepartmentDal departmentDal)
        {
            _authorizedDal = authorizedDal;
            _studentDal = studentDal;
            _companyDal = companyDal;
            _pdfHelper = helper;
            _databaseContext = databaseContext;
            _logDal = logDal;
            _departmentDal = departmentDal;
        }
        
        [HttpGet]
        public IActionResult AuthorizedApproval()
        {
            ViewBag.Departments = _departmentDal.GetAll();

            var authorizedMail = HttpContext.Session.GetString("Username");
            var authorizedApproval = _authorizedDal.CheckStudentStatus(authorizedMail,1);

            return View(authorizedApproval);
        }

        [HttpGet]
        public IActionResult AuthorizedRejected()
        {
            ViewBag.Departments = _departmentDal.GetAll();

            var authorizedMail = HttpContext.Session.GetString("Username");
            var authorizedApproval = _authorizedDal.CheckStudentStatus(authorizedMail, 0);

            return View(authorizedApproval);
        }

        [HttpPost]
        public IActionResult AuthorizedApprovalOnayla(int id)
        {
            var student = _studentDal.GetStudentById(id);
            var companyName = student.CompanyName;
            if (student.StateId == 2)
            {
                var companyEmail = student.CompanyEmail;
                bool passwordExists = _companyDal.CheckCompanyUserExistPassword(companyEmail);

                if (passwordExists)
                {
                    var loginUrl = Url.Action("Login", "Account", null, Request.Scheme);
                    var subjectMail1 = "Şirketinize Yeni Bir Staj Başvurusu Var!";
                    var mailBody1 = $"Sayın Yetkili,\n\nŞirketinize yeni bir staj başvurusu yapılmıştır. Başvuruyu incelemek ve gerekli formu doldurmak için lütfen aşağıdaki bağlantıya tıklayarak sisteme giriş yapınız. Formun 3 gün içinde doldurulması gerektiğini ve aksi takdirde staj başvurusunun iptal edileceğini hatırlatmak isteriz.\n\nSisteme Giriş: {loginUrl}\n\nSaygılarımızla,\n{companyName}";
                    try
                    {
                        _companyDal.SendMailAsync(companyEmail, null, mailBody1, subjectMail1);
                        TempData["Message"] = "Mail gönderme başarılı";
                    }
                    catch
                    {
                        TempData["Message"] = "Mail gönderme başarısız";
                    }
                }
                else
                {                 
                    string token;
                    bool userExists = _companyDal.CheckCompanyUserExist(companyEmail);

                    if (userExists)
                    {
                        token = _companyDal.GetToken(companyEmail);
                    }
                    else
                    {
                        _companyDal.AddMail(companyEmail, student.CompanyName);
                        token = Guid.NewGuid().ToString();
                        _companyDal.UpdateResetToken(companyEmail, token);
                    }

                    var subjectMail = "Şirketinize Yeni Bir Staj Başvurusu Var!";
                    var mailBody = $"Sayın Yetkili,\n\nŞirketinize yeni bir staj başvurusu yapılmıştır. Ancak, sistemimizde kayıtlı değilsiniz. Şifrenizi oluşturmak ve başvuruyu incelemek için lütfen aşağıdaki bağlantıya tıklayınız. Şifrenizi 3 gün içinde oluşturmanız gerektiğini ve aksi takdirde staj başvurusunun iptal edileceğini hatırlatmak isteriz.\n\nŞifre Oluşturma: {Url.Action("CreatePassword", "Company", new { token = token }, Request.Scheme)}\n\nSaygılarımızla,\n{companyName}";

                    try
                    {
                        _companyDal.SendMailAsync(companyEmail, token, mailBody, subjectMail);
                        TempData["Message"] = "Mail gönderme başarılı";
                    }
                    catch
                    {
                        TempData["Message"] = "Mail gönderme başarısız";
                    }

                }
            }

            _studentDal.UpdateAcceptState(id, student.StateId);
         
            var studentAd = student.Name;
            var subject = "Staj Başvurunuz Hakkında Bilgilendirme!";
            var roleName = _authorizedDal.RoleFindAuthorizedPerson(HttpContext.Session.GetString("Username"));
            var mailUpdate = $"Sayın {studentAd},\n\n" +
                 $"Staj başvurunuz {roleName} tarafından onaylanmıştır. " +
                 "Stajınızın onaylanmış olması sebebiyle gerekli bilgilendirme ve yönlendirmeler en kısa sürede tarafınıza iletilecektir.\n\n" +
                 "Başarılar dileriz.\n\n" +
                 "Saygılarımızla,\n" +
                 "Bolu Abant İzzet Baysal Üniversitesi\n";
            _studentDal.SendMailAsync(student.StudentEmail, mailUpdate, subject);

            var username = HttpContext.Session.GetString("Username");
            _logDal.Add(DateTime.Now, username, student.StudentEmail + " kişisinin stajını onayladı.");

            return RedirectToAction("AuthorizedApproval", "Authorized");         
        }

        [HttpPost]
        public IActionResult VeriAlRejected(string description, int id)
        {
            try
            {
                var student = _studentDal.GetStudentById(id);
                var studentAd = student.Name;
                _studentDal.UpdateRejectState(id, description);
                var subject = "Staj Başvurunuz Hakkında Bilgilendirme!";
                var mailBody = $"Sayın {studentAd},\n\n" +
                       "Üniversitemize yapmış olduğunuz staj başvurusu titizlikle değerlendirilmiştir. " +
                       "Ancak, üzülerek belirtmek isteriz ki başvurunuz olumsuz sonuçlanmıştır.\n\n" +
                       $"Reddedilme nedeni: {description}\n\n" +
                       "Saygılarımızla,\n" +
                       "Bolu Abant İzzet Baysal Üniversitesi\n";
                _studentDal.SendMailAsync(student.StudentEmail, mailBody, subject);

                var username = HttpContext.Session.GetString("Username");
                _logDal.Add(DateTime.Now, username, student.StudentEmail + " kişisinin stajını reddetti.");

                return RedirectToAction(nameof(AuthorizedApproval));
            }
            catch (Exception ex)
            {
                // Hata durumunda yapılacak işlemler
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                return RedirectToAction(nameof(AuthorizedApproval));
            }
        }
        public FileResult GetPdf(int id)
        {
            var authorizedMail = HttpContext.Session.GetString("Username");
            var authorizedRole = _authorizedDal.RoleFindAuthorizedPersonPdf(authorizedMail);
            try
            {
                var document = _pdfHelper.GetPdf(id, authorizedRole);
                var array = document.GeneratePdf();
                var student = _pdfHelper.GetStudent(id);
                _logDal.Add(DateTime.Now, authorizedMail, student.StudentEmail + " kişisinin pdfini oluşturdu.");
                return File(array, "application/pdf", $"{student.Number}.pdf");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IActionResult VeriAl()
        {
            throw new NotImplementedException();
        }
        public async Task<IActionResult> IsgDisplay(int id)
        {
            var student = await _databaseContext.Student.FindAsync(id);
            if (student == null || string.IsNullOrEmpty(student.Isg))
                return NotFound();

            var filePath = student.Isg;
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/pdf", Path.GetFileName(filePath));
        }

        public async Task<IActionResult> ProvuzyonDisplay(int id)
        {
            var student = await _databaseContext.Student.FindAsync(id);
            if (student == null || string.IsNullOrEmpty(student.Provuzyon))
                return NotFound();

            var filePath = student.Provuzyon;
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/pdf", Path.GetFileName(filePath));
        }

        public async Task<IActionResult> IntermFileDisplay(int id)
        {
            var student = await _databaseContext.Student.FindAsync(id);
            if (student == null || string.IsNullOrEmpty(student.InTermFile))
                return NotFound();

            var filePath = student.InTermFile;
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/pdf", Path.GetFileName(filePath));
        }

        public IActionResult AuthorizedApproved()
        {
            ViewBag.Departments = _departmentDal.GetAll();
                
            var authorizedMail = HttpContext.Session.GetString("Username");
            var authorizedRoleId = _authorizedDal.RoleIdFindAuthorizedPerson(authorizedMail);
            var authorizedApprovedList = _authorizedDal.AuthorizedApproved(authorizedRoleId, authorizedMail);

            return View(authorizedApprovedList);
        }
    }
}
