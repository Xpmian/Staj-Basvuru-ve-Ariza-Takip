using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Context;
using Aibu.InternshipAutomation.Data.Entities;
using Aibu.InternshipAutomation.Helper;
using Aibu.InternshipAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Aibu.InternshipAutomation.Controllers
{
    [Authorize(Roles = "Öğrenci")]
    public class StudentController : Controller
    {
        private readonly IStudentDal _studentDal;
        private readonly ICompanyDal _companyDal;
        private readonly DatabaseContext _databaseContext;
        private readonly IDepartmentDal _departmentDal;
        private readonly IAuthorizedDal _authorizedDal;
        private readonly ILogDal _logDal;
        private readonly IWebHostEnvironment _environment;
        private readonly IPdfHelper _pdfHelper;
        public StudentController(IStudentDal studentDal, ICompanyDal companyDal, DatabaseContext context, IDepartmentDal departmentDal, IAuthorizedDal authorizedDal, ILogDal logDal, IWebHostEnvironment environment, IPdfHelper helper)
        {
            _studentDal = studentDal;
            _companyDal = companyDal;
            _databaseContext = context;
            _departmentDal = departmentDal;
            _authorizedDal = authorizedDal;
            _logDal = logDal;
            _pdfHelper = helper;
            _environment = environment;
        }
        public IActionResult Past()
        {
            var studentNumber = HttpContext.Session.GetString("Number");
            var pastInternShipList = _studentDal.PastInternShip(studentNumber);
            return View(pastInternShipList);
        }
        public IActionResult Apply()
        {
            ViewBag.Cities = _databaseContext.City.AsNoTracking();
            ViewBag.InternPeriods = _databaseContext.InternPeriod.AsNoTracking();
            ViewBag.InternTypes = _databaseContext.InternTypes.AsNoTracking();
            ViewBag.Departments = _departmentDal.GetAll();

            Boolean staj1AcikMi = true; //Staj 1 açık olacaksa true olmayacaksa false gönder
            Boolean staj2AcikMi = true; //Staj 2 açık olacaksa true olmayacaksa false gönder

            var studentNumber = HttpContext.Session.GetString("Number");
            staj1AcikMi = _studentDal.InternType1(studentNumber);
            staj2AcikMi = _studentDal.InternType2(studentNumber);

            ViewBag.Staj1 = staj1AcikMi;
            ViewBag.Staj2 = staj2AcikMi;

            return View();
        }

        public IActionResult Status()
        {
            return View();
        }
        public IActionResult Completed()
        {
            var studentEmail = HttpContext.Session.GetString("Username");
            string studentNumber = _studentDal.ExtractStudentNumber(studentEmail);
            var studentInfoList = _studentDal.GetStudentForPdf(studentNumber);
            return View(studentInfoList);
        }

        public IActionResult Continuing()
        {
            var studentEmail = HttpContext.Session.GetString("Username");
            string studentNumber = _studentDal.ExtractStudentNumber(studentEmail);
            var studentInfoList = _studentDal.GetStudentContinuing(studentNumber);
            return View(studentInfoList);
        }
        [HttpPost]
        public IActionResult Cancel( int id)
        {
            try
            {
                var student = _studentDal.GetStudentById(id);
                var studentAd = student.Name;
                _studentDal.RejectStudent(id);

                var username = HttpContext.Session.GetString("Username");
                _logDal.Add(DateTime.Now, username,"Staj başvurusunu iptal etti");

                return RedirectToAction(nameof(Continuing));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                return RedirectToAction(nameof(Continuing));
            }
        }

        public IActionResult GetStatus()
        {
            var studentNumber = HttpContext.Session.GetString("Number");
            var statusList = _studentDal.GetAllState(studentNumber);
            var stateList = new List<int> { 0, 0 };

            foreach (var stat in statusList)
            {
                if (stat.TypeId == 1)
                {
                    stateList[0] = stat.StateId - 2;
                }
                else if (stat.TypeId == 2)
                {
                    stateList[1] = stat.StateId - 2;
                }
            }

            return Ok(stateList);
        }
        public IActionResult Student()
        {           
            return View();
        }

        [HttpPost]
        public IActionResult Apply(StudentModel model)
        {         
            return RedirectToAction(nameof(Apply));
        }


        [HttpPost]
        public IActionResult VeriAl(StudentModel model)
        {
            var username = HttpContext.Session.GetString("Username");
            string userEmail = model.CompanyEmail;
            string companyName = model.CompanyName;
            string Date = model.Date;

            string[] dateArray = Date.Split(", ");
            DateTime[] dates = Array.ConvertAll(dateArray, DateTime.Parse);

            Array.Sort(dates);

            DateTime startDate = dates[0];
            DateTime endDate = dates[dates.Length - 1];

            DateTime internShipStartDate = startDate;
            DateTime internShipEndDate = endDate;

            string imagePath = SaveFile(model.ImageData,model.Number,model.InternTypesId);
            string isgPath = SaveFile(model.Isg,model.Number,model.InternTypesId);
            string inTermFilePath = SaveFile(model.InTermFile,model.Number,model.InternTypesId);
            string provuzyonPath = SaveFile(model.Provuzyon, model.Number,model.InternTypesId);

            var student = new Students()
            {
                Name = model.Name,
                Number = model.Number,
                Surname = model.Surname,
                Class = model.Class,
                IdentificationNumber = model.IdentificationNumber,
                StudentEmail = model.StudentEmail,
                CompanyName = model.CompanyName,
                CompanyEmail = model.CompanyEmail,
                Address = model.Address,
                TelephoneNumber = model.TelephoneNumber,
                DateOfBirth = model.DateOfBirth,
                MotherName = model.MotherName,
                FatherName = model.FatherName,
                InternTypesId = model.InternTypesId,
                Date = Date,
                IntershipStartDate = internShipStartDate,
                IntershipEndDate = internShipEndDate,
                InternPeriodId = model.InternPeriodId,
                StateId = 2,
                AcceptanceStatusId = 1,
                PlaceOfBirthId = model.PlaceOfBirthId,
                DepartmentId = model.DepartmentId,
                ImageData = imagePath,
                Isg = isgPath,
                InTermFile = inTermFilePath,
                Provuzyon = provuzyonPath,
                ImageMimeType = model.ImageMimeType,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            var entity = _studentDal.Add(student);

            if (entity is null)
            {
                _logDal.Add(DateTime.Now, model.StudentEmail, " Staj başvurusu yapamadı.");
                return BadRequest("Veri Eklenemedi");
            }

            _logDal.Add(DateTime.Now, model.StudentEmail, " Staj başvurusu yaptı.");
            return RedirectToAction("Status", "Student");
        }
        public IActionResult Veri()
        {
            Students students = _databaseContext.Student.Find(6);
            throw new NotImplementedException();
            // return View(students);

        }
        private string SaveFile(IFormFile file, string number, int internTypesId)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var uploadPath = Path.Combine("C:\\uploads");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var folderName = $"{number}-{internTypesId}";
            var numberFolderPath = Path.Combine(uploadPath, folderName);

            if (!Directory.Exists(numberFolderPath))
            {
                Directory.CreateDirectory(numberFolderPath);
            }

            var newFileName = $"{number}-{internTypesId}-{Path.GetFileNameWithoutExtension(file.FileName)}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(numberFolderPath, newFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return filePath;
        }

        public FileResult GetPdf(int id)
        {
            var studentMail = HttpContext.Session.GetString("Username");
            try
            {
                var document = _pdfHelper.GetPdf(id, "Öğrenci");
                var array = document.GeneratePdf();
                var student = _pdfHelper.GetStudent(id);
                _logDal.Add(DateTime.Now, studentMail, student.StudentEmail + " kişisi pdfini oluşturdu.");
                return File(array, "application/pdf", $"{student.Number}.pdf");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
