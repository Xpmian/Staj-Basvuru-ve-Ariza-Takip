using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Entities;
using Aibu.InternshipAutomation.Helper;
using Aibu.InternshipAutomation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using QuestPDF.Fluent;
using System.Diagnostics;

namespace Aibu.InternshipAutomation.Controllers
{
    [Authorize(Roles = "Şirket")]
    public class CompanyController : Controller
    {
        private readonly ICompanyDal _companyDal;
        private readonly IStudentDal _studentDal;
        private readonly ILogDal _logDal;
        private readonly IPdfHelper _pdfHelper;
        private readonly IAuthorizedDal _authorizedDal;

        public CompanyController(ICompanyDal companyDal, IStudentDal studentDal, ILogDal logDal, IPdfHelper pdfHelper, IAuthorizedDal authorizedDal)
        {
            _companyDal = companyDal;
            _studentDal = studentDal;
            _logDal = logDal;
            _pdfHelper = pdfHelper;
            _authorizedDal = authorizedDal;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreatePassword(string Password, string a)
        {

            var tokenuser = TempData["token"];
            try
            {
                _companyDal.UpdateUser(tokenuser.ToString(), Password);

                TempData["Status"] = "success";
                TempData["Message"] = "Şifre başarıyla Güncellendi.";
            }
            catch
            {
                TempData["Status"] = "error";
                TempData["Message"] = "Şifre güncelleme başarısız oldu.";
            }


            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult CreatePassword(string token)
        {
            TempData["token"] = token;
            return View();

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string Password, string a)
        {

            var tokenuser = TempData["token"];
            try
            {
                _companyDal.UpdateUser(tokenuser.ToString(), Password);

                TempData["Status"] = "success";
                TempData["Message"] = "Şifre başarıyla Güncellendi.";
            }
            catch
            {
                TempData["Status"] = "error";
                TempData["Message"] = "Şifre güncelleme başarısız oldu.";
            }


            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            TempData["token"] = token;
            return View();

        }

        public IActionResult CompanyApplicant()
        {
            var companyEmail = HttpContext.Session.GetString("Username");
            var companyApplicantList = _companyDal.GetApplicantStudentByCompanyEmail(companyEmail);

            ViewBag.Company = true;

            return View(companyApplicantList);

        }

        public IActionResult CompanyInformation()
        {
            var companyEmail = HttpContext.Session.GetString("Username");
            var company = _companyDal.CheckCompany(companyEmail);
            var companyList = _companyDal.CompanyIdByEmail1(companyEmail);
            Boolean companyVar_mı = true;
            ViewBag.Company = companyVar_mı;
            ViewBag.AllDayWorkingOnWeekends = true;

            ViewBag.ComputerEngineer = true;
            ViewBag.ElectricalElectronicsEngineering = true;
            ViewBag.MechanicalEngineering = true;
            ViewBag.CivilEngineering = true;
            ViewBag.FoodEngineering = true;
            ViewBag.EnvironmentalEngineering = true;
            ViewBag.ChemicalEngineering = true;

            if (companyList[0].AllDayWorkingOnWeekends == false)
            {
                ViewBag.AllDayWorkingOnWeekends = false;
            }

            if (companyList[0].IsComputerEngineer == false)
            {
                ViewBag.ComputerEngineer = false;
            }
            if (companyList[0].IsElectricalElectronicsEngineering == false)
            {
                ViewBag.ElectricalElectronicsEngineering = false;
            }
            if (companyList[0].IsMechanicalEngineering == false)
            {
                ViewBag.MechanicalEngineering = false;
            }
            if (companyList[0].IsCivilEngineering == false)
            {
                ViewBag.CivilEngineering = false;
            }
            if (companyList[0].IsFoodEngineering == false)
            {
                ViewBag.FoodEngineering = false;
            }
            if (companyList[0].IsEnvironmentalEngineering == false)
            {
                ViewBag.EnvironmentalEngineering = false;
            }
            if (companyList[0].IsChemicalEngineering == false)
            {
                ViewBag.ChemicalEngineering = false;
            }

            var viewModel = new CompanyViewModel
            {
                CompanyInfoView = companyList
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult CompanyInformation(CompanyViewModel model)
        {
            return RedirectToAction(nameof(CompanyInformation));
        }

        [HttpPost]
        public IActionResult VeriAlCompany(CompanyViewModel model)
        {
            var companyId = HttpContext.Session.GetString("CompanyId");
            var username = HttpContext.Session.GetString("Username");
            int companyId1 = int.Parse(companyId);

            bool IsComputerEngineer = new List<bool>
            {
                model.CompanyUser.IsComputerEngineer == true,
                model.CompanyUser.IsSoftwareEngineer == true,
                model.CompanyUser.IsIt == true,
                model.CompanyUser.IsArtificialIntelligenceEngineer == true
            }.Any(condition => condition);

            bool IsElectricalElectronicsEngineering = new List<bool>
            {
                model.CompanyUser.IsElectricalEngineer == true,
                model.CompanyUser.IsElectronicsEngineer == true,
                model.CompanyUser.IsElectronicCommunicationEngineer == true,
                model.CompanyUser.IsElectricalElectronicsEngineer == true
            }.Any(condition => condition);

            bool IsMechanicalEngineering = new List<bool>
            {
                model.CompanyUser.IsEMechanicalEngineer == true,
                model.CompanyUser.IsEMechatronicsEngineer == true,
                model.CompanyUser.IsFlightEngineer == true,
                model.CompanyUser.IsMaterialsEngineer == true,
                model.CompanyUser.IsPowerEngineer == true,
                model.CompanyUser.IsVehicleEngineer == true,
                model.CompanyUser.IsProcessEngineer == true,
                model.CompanyUser.IsPowerPlantEngineer == true,
                model.CompanyUser.IsEnergySystemsEngineer == true,
                model.CompanyUser.IsNuclearEnergyEngineer == true,
                model.CompanyUser.IsPetroleumAndNaturalGasEngineer == true,
                model.CompanyUser.IsIndustrialEngineerMechanical == true,
                model.CompanyUser.IsMarineEngineer == true,
                model.CompanyUser.IsAutomotiveEngineer == true,
                model.CompanyUser.IsAerospaceEngineer == true
            }.Any(condition => condition);

            bool IsCivilEngineering = new List<bool>
            {
                model.CompanyUser.IsCivilEngineer == true,
                model.CompanyUser.IsArchitect == true
            }.Any(condition => condition);

            bool IsFoodEngineering = new List<bool>
            {
                model.CompanyUser.IsFoodEngineer == true,
                model.CompanyUser.IsIndustrialEngineer == true,
                model.CompanyUser.IsMechanicalEngineerFood == true,
                model.CompanyUser.IsFisheriesEngineer == true     
            }.Any(condition => condition);

            bool IsEnvironmentalEngineering = new List<bool>
            {
                model.CompanyUser.IsEnvironmentalEngineer == true,
                model.CompanyUser.IsAnyEngineer == true
            }.Any(condition => condition);

            bool IsChemicalEngineering = new List<bool>
            {
                model.CompanyUser.IsComputerEngineer == true,
                model.CompanyUser.IsSoftwareEngineer == true,
                model.CompanyUser.IsIt == true
            }.Any(condition => condition);


            var requiredEngineers = new List<string>();

            if (IsComputerEngineer == true)
            {
                requiredEngineers.Add("Bilgisayar Mühendisi");
            }
            if (IsElectricalElectronicsEngineering == true)
            {
                requiredEngineers.Add("Elektrik Elektronik Mühendisi");
            }
            if (IsMechanicalEngineering == true)
            {
                requiredEngineers.Add("Makina Mühendisi");
            }
            if (IsCivilEngineering == true)
            {
                requiredEngineers.Add("İnşaat Mühendisi");
            }
            if (IsFoodEngineering == true)
            {
                requiredEngineers.Add("Gıda Mühendisi");
            }
            if (IsEnvironmentalEngineering == true)
            {
                requiredEngineers.Add("Çevre Mühendisi");
            }
            if (IsChemicalEngineering == true)
            {
                requiredEngineers.Add("Kimya Mühendisi");
            }

            string ExistingEngineers = string.Join(", ", requiredEngineers);

            var companyUserss = new CompanyUserss
            {
                EmployeeName = model.CompanyUser.EmployeeName,
                Title = model.CompanyUser.Title,
                MissionArea = model.CompanyUser.MissionArea,
                Telephone = model.CompanyUser.Telephone,
                Adress = model.CompanyUser.Adress,
                ActivityArea = model.CompanyUser.ActivityArea,
                TotalNumberOfEmployees = model.CompanyUser.TotalNumberOfEmployees,
                AllDayWorkingOnWeekends = model.CompanyUser.AllDayWorkingOnWeekends,
                TelephoneCompany = model.CompanyUser.TelephoneCompany,
                Fax = model.CompanyUser.Fax,
                CompanyEmail = model.CompanyUser.CompanyEmail,
                IsComputerEngineer = IsComputerEngineer,
                IsElectricalElectronicsEngineering = IsElectricalElectronicsEngineering,
                IsMechanicalEngineering = IsMechanicalEngineering,
                IsCivilEngineering = IsCivilEngineering,
                IsFoodEngineering = IsFoodEngineering,
                IsEnvironmentalEngineering = IsEnvironmentalEngineering,
                IsChemicalEngineering = IsChemicalEngineering,
                ExistingEngineers = ExistingEngineers
            };

            var entity2 = _companyDal.CompanyUserUpdate(companyUserss, companyId1);

            if (entity2 is null)
            {
                _logDal.Add(DateTime.Now, username, " Şirket veri ekleyemedi.");
                TempData["Status"] = "success";
                TempData["Message"] = "Bigiler kayıt edilemedi";
                return BadRequest("Veri Eklenemedi");

            }
            TempData["Status"] = "success";
            TempData["Message"] = "Bigiler Başarılı bir şekilde kayıt edildi";
            _logDal.Add(DateTime.Now, username, " Şirket veri girişi yaptı.");
            return RedirectToAction("CompanyInformation", "Company");
        }

        [HttpPost]
        public IActionResult CompanyReject(string description, int id)
        {
            try
            {
                var student = _studentDal.GetStudentById(id);
                _studentDal.UpdateRejectState(id, description);
                var subject = "Stajınız Reddedildi!";
                var mailBody = "Staj başvurunuz aşağıdaki nedenden dolayı reddedilmiştir lütfen birdaha başvuru yapınız:\n " + description;
                _studentDal.SendMailAsync(student.StudentEmail, mailBody, subject);

                var username = HttpContext.Session.GetString("Username");
                _logDal.Add(DateTime.Now, username, "Şirket " + student.StudentEmail + " kişisinin stajını reddetti.");

                return RedirectToAction(nameof(CompanyApplicant));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                return RedirectToAction(nameof(CompanyApplicant));
            }
        }
        [HttpPost]
        public IActionResult CompanyApprovalOnayla(int id)
        {
            var student = _studentDal.GetStudentById(id);
            _companyDal.UpdateAcceptState(id, student.StateId);

            var mailUpdate = "Stajınız Şirket tarafından onaylandı.";
            var subject = "Staj Başvurunuzda Güncelleme";
            _studentDal.SendMailAsync(student.StudentEmail, mailUpdate, subject);

            var username = HttpContext.Session.GetString("Username");
            _logDal.Add(DateTime.Now, username, "Şirket " + student.StudentEmail + " kişisinin stajını onayladı.");

            return RedirectToAction("CompanyApplicant", "Company");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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

        public IActionResult CompanyApproved()
        {
            var companyEmail = HttpContext.Session.GetString("Username");
            var company = _companyDal.CheckCompany(companyEmail);
            if (company == false)
            {
                ViewBag.Company = false;
            }
            else
            {
                ViewBag.Company = true;
                var companyApprovedList = _companyDal.CompanyApproved(companyEmail);

                return View(companyApprovedList);
            }
            return View();
        }
    }
}