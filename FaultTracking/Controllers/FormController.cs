using FaultTracking.Data.Base;
using FaultTracking.Data.Context;
using FaultTracking.Data.Entities;
using FaultTracking.Helper;
using FaultTracking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace FaultTracking.Controllers
{
    public class FormController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IFormDal _formDal;
        private readonly IPdfHelper _pdfHelper;
        private readonly ILogDal _logDal;
        public FormController(DatabaseContext context, IFormDal formDal , IPdfHelper pdfHelper, ILogDal logDal)
        {
            _databaseContext = context;
            _formDal = formDal;
            _pdfHelper = pdfHelper;
            _logDal = logDal;
        }
        [Authorize(Roles = "Diger,Admin")]
        public IActionResult Create()
        {
            ViewBag.Type = _formDal.GetAllFaultTypes();
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var authorizedApproval = _formDal.GetAll();
            return View(authorizedApproval);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Excel()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult WaitingFaults()
        {
            var waitingFaultList = _formDal.WaitingFault();
            ViewBag.AuthorizedPerson = _formDal.GetAuthorizedPerson();
            ViewBag.Colour = _formDal.GetAllColourCode();
            return View(waitingFaultList);
        }

        [Authorize(Roles = "Görevli,Admin")]
        public IActionResult CompletedFaults()
        {
            ViewBag.Colour = _formDal.GetAllColourCode();
            var userMail = HttpContext.Session.GetString("Username");      
            var role = _formDal.RoleFaultList(userMail);

            if (role[0].Role.Equals("Admin"))
            {
                var completedFaultList = _formDal.CompletedFault();
                return View(completedFaultList);
            }
            else
            {
                var completedFaultsList = _formDal.PersonelCompaletedFaultList(userMail);
                return View(completedFaultsList);
            }           
        }

        [Authorize(Roles = "Görevli,Admin")]
        public IActionResult AppointedFault()
        {
            ViewBag.Colour = _formDal.GetAllColourCode();
            var userMail = HttpContext.Session.GetString("Username");
            var role = _formDal.RoleFaultList(userMail);
            ViewBag.Colour = _formDal.GetAllColourCode();

            if (role[0].Role.Equals("Admin"))
            {
                var appointedFaultList = _formDal.AppointedFault();
                return View(appointedFaultList);
            }
            else 
            {
                var appointedFaultList = _formDal.PersonelAppointedFaultList(userMail);
                return View(appointedFaultList);
            }
        }
       
        public IActionResult ContinueprocessOwner()
        {
            var userMail = HttpContext.Session.GetString("Username");
            var faultList = _formDal.ContinueprocessOwner(userMail);

            return View(faultList);
        }
   
        public IActionResult CompletedprocessOwner()
        {
            var userMail = HttpContext.Session.GetString("Username");
            var faultList = _formDal.CompletedprocessOwner(userMail);

            return View(faultList);
        }


        [HttpPost]
        public async Task<IActionResult> VeriAlAsync(FormModel model)
        {
            var userMail = HttpContext.Session.GetString("Username");
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("dd/MM/yyyy");
            string [] studenNumberArray = userMail.Split('@');
            string studentNumber = studenNumberArray[0];

            var form = new Forms()
            {
                Field = model.Field,
                Title = model.Title,
                FaultTypeId = model.FaultTypeId,
                Description = model.Description,
                Date = formattedDate,
                UserMail = userMail,
                FormStatusId = 1,
                studentNumber = studenNumberArray[0]
            };

            var studentName = "";
            if (userMail.EndsWith("@ogrenci.ibu.edu.tr"))
            {
                studentName = _formDal.GetStudent(studentNumber);
            }
            else
            {
                studentName = studenNumberArray[0];
            }
                
            var entity = _formDal.Add(form);

            if (entity is null)
            {
                return BadRequest("Veri Eklenemedi");
            }

            TempData["Status"] = "success";
            TempData["Message"] = "Arıza kaydı başarıyla oluşturuldu.";

            var email = _databaseContext.AuthorizedPerson.Where(p => p.FaultRoleId == 1).FirstOrDefault().Email;
            var subject = "Arıza Bildirimi";
            var mailUpdate = $"Sayın Fakülte Sekreteri,\n" +
                          $"Bu e-posta, {studentName} adlı öğrencinin aşağıda belirtilen arıza kaydını bildirmek için gönderilmiştir.\n" +
                          $"Arıza Kayıt Tarihi: {formattedDate}\n" +
                          $"Arıza Detayları: {model.Title}\n" +
                          $"Gerekli incelemelerin ve işlemlerin yapılmasını rica ederim.\n" +
                          $"Saygılarımla";
            await _formDal.SendMailAsync(email, mailUpdate, subject);

            _logDal.Add(DateTime.Now, userMail, "Form Oluşturdu");
            return RedirectToAction("Create", "Form");
        }

        [HttpPost]
        public IActionResult AuthorizedApprovalOnayla(int authorizedPersonId, int id , int colourId)
        {
            _formDal.UpdateApprovalStatus(id, authorizedPersonId, colourId);
            _logDal.Add(DateTime.Now, "Admin", "Arıza Atandı");
            return RedirectToAction("WaitingFaults", "Form");
        }


        [HttpPost]
        public async Task<IActionResult> AuthorizedApprovalTamamlaAsync(int id)
        {
            var form = _formDal.GetFormtById(id);
            var mail = form.UserMail;
            var mailUpdate = "Bildirmiş olduğunuz arıza giderilmiştir.";
            var subject = "Bildirmiş olduğunuz arıza giderilmiştir.";
            await _formDal.SendMailAsync(mail, mailUpdate, subject);

            var userMail = HttpContext.Session.GetString("Username");
            var role = _formDal.RoleFaultList(userMail);
            if(role[0].Role.Equals("Admin"))
            {
                return RedirectToAction("AppointedFault", "Form");
            }
            else
            {
                _formDal.UpdateApprovalAttendantStatus(id);
                _logDal.Add(DateTime.Now, userMail, "Arıza Giderildi");
                return RedirectToAction("AppointedFault", "Form");
            }
        }
        public FileResult GetPdf(int id)
        {
            try
            {
                var document = _pdfHelper.GetPdf(id);
                var array = document.GeneratePdf();
                var student = _pdfHelper.GetFaults(id);
                _logDal.Add(DateTime.Now, "Admin" , "Pdf Oluşturuldu");
                return File(array, "application/pdf", $"{student.Title}.pdf");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}