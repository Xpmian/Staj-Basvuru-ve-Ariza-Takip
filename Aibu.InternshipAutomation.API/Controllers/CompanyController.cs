using Aibu.InternshipAutomation.API.Model;
using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Aibu.InternshipAutomation.API.Controllers
{
    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        private readonly ICompanyDal _companyDal;
        public CompanyController(ICompanyDal companyDal)
        {
            _companyDal = companyDal;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_companyDal.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_companyDal.GetCompanyById(id));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _companyDal.Delete(id);
            if (entity is null)
                return BadRequest("Hata olustu");

            return Ok("Silme Islemi Basariyla Tamamlandi");
        }

        [HttpPost]
        public IActionResult Post([FromBody] CompanyModel model)
        {
            var companies = new Companies()
            {
                CompanyName = model.CompanyName,
                Adress = model.Adress,
                ActivityArea = model.ActivityArea,
                TotalNumberOfEmployees = model.TotalNumberOfEmployees,
                AllDayWorkingOnWeekends = model.AllDayWorkingOnWeekends,
                TelephoneCompany = model.TelephoneCompany,
                Fax = model.Fax,
                EmployeerName = model.EmployeerName,
                Telephone = model.Telephone,
                MissionArea = model.MissionArea,
                Email = model.Email,
                Password = model.Password,
                CreateTime = model.CreateTime,
                UpdateTime = model.UpdateTime,
                IsActive = model.IsActive,
                RoleId=model.RoleId
            };
            var entity = _companyDal.Add(companies);
            if (entity is null)
            {
                return BadRequest("Veri Eklenemedi");
            }
             return Ok("Veri Ekleme isi Basarili");
        }
    }
}
