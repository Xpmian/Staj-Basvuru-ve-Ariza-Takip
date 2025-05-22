using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FaultTracking.Data.Base;
using System.Security.Claims;
using System.Text;
using FaultTracking.Models;
using Newtonsoft.Json;
using System.Net;

namespace FaultTracking.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ILoginDal _loginDal;
        private readonly ILogDal _logDal;


        public AccountController(ILoginDal loginDal, ILogDal logDal)
        {
            _loginDal = loginDal;
            _logDal = logDal;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Diger"))
                {
                    var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                   
                    HttpContext.Session.Set("Username", Encoding.ASCII.GetBytes(userEmail));
                    _logDal.Add(DateTime.Now, userEmail, "Sisteme tekrardan girdi.");

                    return RedirectToAction("Create", "Form");
                }
                else if (User.IsInRole("Admin"))
                {
                    HttpContext.Session.Set("Username", Encoding.ASCII.GetBytes("Admin"));
                    _logDal.Add(DateTime.Now, "Admin", "Sisteme tekrardan girdi.");

                    return RedirectToAction("AdminAuthorized", "Admin");
                }
                else if (User.IsInRole("Görevli"))
                {
                    var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                    HttpContext.Session.Set("Username", Encoding.ASCII.GetBytes(userEmail));
                    _logDal.Add(DateTime.Now, userEmail, "Sisteme tekrardan girdi.");

                    return RedirectToAction("AppointedFault", "Form");
                }
                //Kullanıcı oturum açtıysa, istediğiniz sayfaya yönlendir

            }
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string captchaResponse = Request.Form["g-recaptcha-response"];
            bool isCaptchaValid = ValidateCaptcha(captchaResponse);

            var username = model.Username;
            string number = username?.Substring(0, Math.Min(username.Length, 9));

            var usernameauthorized = username.Split('@')[0];

            var password = model.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["Status"] = "warning";
                TempData["Message"] = "Kullanıcı adı veya şifre boş olamaz!";
                return View(model);
            }
            else if (ModelState.IsValid)
            {
                if (isCaptchaValid)
                {
                    if (username.EndsWith("@ogrenci.ibu.edu.tr"))
                    {
                        string encodedPassword = Uri.EscapeDataString(password);
                        var response = _loginDal.Auth(number, encodedPassword, "ogrenci");
                        if (response.Equals("e"))
                        {
                            await Authenticate(model, "Diger");
                            TempData["Status"] = "success";
                            TempData["Message"] = "Giriş başarılı";
                            _logDal.Add(DateTime.Now, username, "Sisteme tekrardan girdi.");
                            return RedirectToAction("Create", "Form");
                        }
                        else if (response.Equals("h"))
                        {
                            TempData["Status"] = "error";
                            TempData["Message"] = "Mail/Sifre hatali";
                            return View(model);
                        }

                    }
                    else if (username.EndsWith("@ibu.edu.tr"))
                    {
                        var roleName = _loginDal.RoleFindAuthorizedPerson(model.Username);
                        string encodedPassword = Uri.EscapeDataString(password);
                        var response = _loginDal.Auth(usernameauthorized, encodedPassword, "personel");
                        if (response.Equals("e"))
                        {
                            await Authenticate(model, roleName);
                            TempData["Status"] = "success";
                            TempData["Message"] = "Giriş başarılı";
                            _logDal.Add(DateTime.Now, username, "Sisteme girdi.");
                            return RedirectToAction("Create", "Form");
                        }
                        else if (response.Equals("h"))
                        {
                            TempData["Status"] = "error";
                            TempData["Message"] = "Mail/Sifre hatali";
                            return View(model);
                        }

                    }               
                    else
                    {
                        TempData["Status"] = "error";
                        TempData["Message"] = "Mail/Sifre hatali";
                        ModelState.Clear();
                    }
                }
                else
                {
                    TempData["Status"] = "warning";
                    TempData["Message"] = "Captcha doğrulaması başarısız. Lütfen tekrar deneyin.";
                }
            }
            else
            {
                TempData["Status"] = "error";
                TempData["Message"] = "Mail/Sifre hatali";
                return View(model);
            }

            return View(model);
        }
        private async Task Authenticate(LoginModel model, string roleName)
        {
            var username = model.Username;
            string number = username?.Substring(0, Math.Min(username.Length, 9));

            if (username.EndsWith("@ogrenci.ibu.edu.tr"))
            {
                var userList = _loginDal.StudentNameFind(number);
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, userList[0].Name), new Claim(ClaimTypes.Role, roleName), new Claim(ClaimTypes.Anonymous, userList[0].Number), new Claim(ClaimTypes.Email, model.Username), new Claim(ClaimTypes.Surname, userList[0].Surname) };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                HttpContext.Session.Set("Username", Encoding.ASCII.GetBytes(model.Username));
            }
            else if (username.EndsWith("@ibu.edu.tr"))
            {
                var authorizedPerson = _loginDal.AuthorizedPersonFind(model.Username);
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, authorizedPerson[0].Name), new Claim(ClaimTypes.Role, roleName), new Claim(ClaimTypes.Email, authorizedPerson[0].Email), new Claim(ClaimTypes.Surname, authorizedPerson[0].Surname) };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                HttpContext.Session.Set("AuthorizedName", Encoding.ASCII.GetBytes(authorizedPerson[0].Name));
                HttpContext.Session.Set("AuthorizedSurname", Encoding.ASCII.GetBytes(authorizedPerson[0].Surname));
                HttpContext.Session.Set("Username", Encoding.ASCII.GetBytes(authorizedPerson[0].Email));
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logoff()
        {
            var userMail = HttpContext.Session.GetString("Username");
            HttpContext.Session.Remove("Username");

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Status"] = "success";
            TempData["Message"] = "Çıkış başarılı";
            _logDal.Add(DateTime.Now, userMail, "Sistemden Çıktı");
            return RedirectToAction("Login", "Account");
        }
        public bool ValidateCaptcha(string response)
        {
            string secret = "6LfTrOcpAAAAAEoOCiPYoGTD6_TU8Sx2K5d1D06Z";

            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            return Convert.ToBoolean(captchaResponse.Success);

        }

        public IActionResult AccessDenied()
        {
            var username = HttpContext.Session.GetString("Username");
            _logDal.Add(DateTime.Now, username, "Erişim engellendi.");
            return View();
        }
    }
}