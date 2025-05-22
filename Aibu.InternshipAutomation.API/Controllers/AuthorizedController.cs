using Aibu.InternshipAutomation.Data.Base;
using Microsoft.AspNetCore.Mvc;

namespace Aibu.InternshipAutomation.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizedController : Controller
    {
        private readonly IAuthorizedDal _authorizedDal;
        public AuthorizedController(IAuthorizedDal authorizedDal)
        {
            _authorizedDal = authorizedDal;
        }
        [HttpGet]
        public IActionResult Get(string email)
        {
            return Ok(_authorizedDal.StudentRejected("aaaa"));
        }

        //[HttpGet]
        //public IActionResult Get(string email)
        //{
        //    return Ok(_authorizedDal.StudentAwaitingApproval("aaaa"));
        //}
    }
}
