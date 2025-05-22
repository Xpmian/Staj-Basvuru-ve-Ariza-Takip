using Aibu.InternshipAutomation.Data.Base;
using Microsoft.AspNetCore.Mvc;

namespace Aibu.InternshipAutomation.API.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly IStudentDal _studentDal;
        public StudentController (IStudentDal studentDal)
        {
            _studentDal = studentDal;
        }
        //[HttpGet]
        //public IActionResult Get(string studentNumber)
        //{
        //    return Ok(_studentDal.InformationByStudentNumber("213405062"));
        //}

        [HttpGet]
        public IActionResult Get(string studentNumber)
        {
            return Ok(_studentDal.PastInternShip("213405061"));
        }
    }
}
