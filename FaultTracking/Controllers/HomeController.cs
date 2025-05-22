using FaultTracking.Data.Base;
using FaultTracking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System.Diagnostics;

namespace FaultTracking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFormDal _formDal;

        public HomeController(ILogger<HomeController> logger, IFormDal formDal)
        {
            _logger = logger;
            _formDal = formDal;
        }

        public IActionResult Index()
        {
            var a = _formDal.GetAll();
            var b = _formDal.CompletedFault();
            var d = _formDal.WaitingFault();
            var c = _formDal.AppointedFault();
            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
