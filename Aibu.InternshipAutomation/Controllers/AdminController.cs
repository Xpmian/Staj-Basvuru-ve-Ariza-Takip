using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Context;
using Aibu.InternshipAutomation.Data.Dal;
using Aibu.InternshipAutomation.Data.Entities;
using Aibu.InternshipAutomation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aibu.InternshipAutomation.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILoginDal _loginDal;
        private readonly ICompanyDal _companyDal;
        private readonly IAuthorizedDal _authorizedDal;
        private readonly ILogDal _logDal;
        private readonly IAdminDal _adminDal;
        private readonly IDepartmentDal _departmentDal;
        private readonly DatabaseContext _databaseContext;

        public AdminController(ILoginDal loginDal, ICompanyDal companyDal, IAuthorizedDal authorizedDal, ILogDal logDal, DatabaseContext databaseContext, IAdminDal adminDal, IDepartmentDal departmentDal)
        {
            _loginDal = loginDal;
            _companyDal = companyDal;
            _authorizedDal = authorizedDal;
            _authorizedDal = authorizedDal;
            _logDal = logDal;
            _databaseContext = databaseContext;
            _adminDal = adminDal;
            _departmentDal = departmentDal;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Excel()
        {
            return View();
        }

        public IActionResult AdminAuthorized() 
        {
            ViewBag.Departments = _departmentDal.GetAll();

            var bolumBaskanı = _authorizedDal.GetAllPerson(4,1);
            var stajKomisyon = _authorizedDal.GetAllPerson(6,1);
            var fakulteSekreteri = _authorizedDal.GetAllPerson(7,1);
            var bolumSekreteri = _authorizedDal.GetAllPerson(8,1);

            var viewModel = new AdminAuthorizedViewModel
            {
                BolumBaskaniList = bolumBaskanı,
                StajKomisyonList = stajKomisyon,
                FakulteSekreteriList = fakulteSekreteri,
                BolumSekreteriList = bolumSekreteri,
            };

            return View(viewModel);
        }

        public IActionResult AdminCompanies()
        {
            var infoList = _companyDal.GetCompanyInfoViews();
         
            return View(infoList);
        }


        [HttpPost]
        public IActionResult VeriAlAuthorizedPersonBolumBaskanı(AdminAuthorizedViewModel model)
        {
            var deparmtenId = model.DepartmentId;
            var entity = _adminDal.UpdateAuthorizedPerson(4,model.AuthorizedPerson.Name, model.AuthorizedPerson.Surname, model.AuthorizedPerson.Email, deparmtenId);
            if (entity is null)
            {
                _logDal.Add(DateTime.Now, "Admin", "Bölüm başkanı olarak eklenmedi");
                return BadRequest("Veri Eklenemedi");
            }

            _logDal.Add(DateTime.Now, "Admin", "Bölüm başkanı olarak eklendi.");
            return RedirectToAction("AdminAuthorized", "Admin");
        }

        [HttpPost]
        public IActionResult VeriAlAuthorizedPersonStajKomisyonu(AdminAuthorizedViewModel model)
        {
            var deparmtenId = model.DepartmentId;
            var entity = _adminDal.UpdateAuthorizedPerson(6,model.AuthorizedPerson.Name, model.AuthorizedPerson.Surname, model.AuthorizedPerson.Email, deparmtenId);
            if (entity is null)
            {
                _logDal.Add(DateTime.Now, "Admin", "Staj komisyonu olarak eklenmedi.");
                return BadRequest("Veri Eklenemedi");
            }

            _logDal.Add(DateTime.Now, "Admin", "Staj komisyonu olarak eklendi.");
            return RedirectToAction("AdminAuthorized", "Admin");
        }

        [HttpPost]
        public IActionResult VeriAlAuthorizedPersonFakülteSekreteri(AdminAuthorizedViewModel model)
        {
            var deparmtenId = model.DepartmentId;
            var entity = _adminDal.UpdateAuthorizedPerson(7,model.AuthorizedPerson.Name, model.AuthorizedPerson.Surname, model.AuthorizedPerson.Email, deparmtenId);
            if (entity is null)
            {
                _logDal.Add(DateTime.Now, "Admin", "Fakülte Sekreteri olarak eklenmedi.");
                return BadRequest("Veri Eklenemedi");
            }

            _logDal.Add(DateTime.Now, "Admin", "Fakülte Sekreteri olarak eklendi");
            return RedirectToAction("AdminAuthorized", "Admin");
        }

        public IActionResult VeriAlAuthorizedPersonBölümSekreteri(AdminAuthorizedViewModel model)
        {
            var deparmtenId = model.DepartmentId;
            var entity = _adminDal.UpdateAuthorizedPerson(8, model.AuthorizedPerson.Name, model.AuthorizedPerson.Surname, model.AuthorizedPerson.Email, deparmtenId);
            if (entity is null)
            {
                _logDal.Add(DateTime.Now, "Admin"  , "Bölüm Sekreteri olarak eklenmedi.");
                return BadRequest("Veri Eklenemedi");
            }

            _logDal.Add(DateTime.Now, "Admin", "Bölüm Sekreteri olarak eklendi.");
            return RedirectToAction("AdminAuthorized", "Admin");
        }

        [HttpPost]
        public IActionResult UpdateInfoStudent(IFormFile excelFile)
        {
            _adminDal.UploadInfoStudent(excelFile);
            _logDal.Add(DateTime.Now, "Admin", "Öğrenci Tablosunu Güncelledi");

            TempData["Status"] = "success";
            TempData["Message"] = "Bigiler Başarılı bir şekilde kayıt edildi";

            return RedirectToAction("Excel", "Admin");
        }

        [HttpPost]
        public IActionResult UpdateInfoAuthorized(IFormFile excelFile)
        {
            _adminDal.UploadInfoAuthorized(excelFile);
            _logDal.Add(DateTime.Now, "Admin", "Yetkili Tablosunu Güncelledi");

            TempData["Status"] = "success";
            TempData["Message"] = "Bigiler Başarılı bir şekilde kayıt edildi";

            return RedirectToAction("Excel", "Admin");
        }

        [HttpPost]
        public IActionResult VeriAlDeparment(AdminAuthorizedViewModel model)
        {
            ViewBag.Departments = _departmentDal.GetAll();

            var deparmtenId = model.DepartmentId;

            var bolumBaskanı = _authorizedDal.GetAllPerson(4,deparmtenId);
            var stajKomisyon = _authorizedDal.GetAllPerson(6,deparmtenId);
            var fakulteSekreteri = _authorizedDal.GetAllPerson(7,deparmtenId);
            var bolumSekreteri = _authorizedDal.GetAllPerson(8,deparmtenId);

            var viewModel = new AdminAuthorizedViewModel
            {
                BolumBaskaniList = bolumBaskanı,
                StajKomisyonList = stajKomisyon,
                FakulteSekreteriList = fakulteSekreteri,
                BolumSekreteriList = bolumSekreteri,
            };

            return View("AdminAuthorized", viewModel);
        }
    }
}
