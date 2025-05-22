using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Context;
using Aibu.InternshipAutomation.Data.Dal;
using Aibu.InternshipAutomation.Models;
using Argon;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Text;

namespace Aibu.InternshipAutomation.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ILoginDal _loginDal;
        private readonly ICompanyDal _companyDal;
        private readonly IAuthorizedDal _authorizedDal;
        private readonly ILogDal _logDal;
        private readonly IStudentDal _studentDal;

        public AccountController(ILoginDal loginDal, ICompanyDal companyDal, IAuthorizedDal authorizedDal, ILogDal logDal, IStudentDal studentDal)
        {
            _loginDal = loginDal;
            _companyDal = companyDal;
            _authorizedDal = authorizedDal;
            _authorizedDal = authorizedDal;
            _logDal = logDal;
            _studentDal = studentDal;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Öğrenci"))
                {
                    var studentEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    string number = studentEmail?.Substring(0, Math.Min(studentEmail.Length, 9));
                    var studentList = _loginDal.StudentNameFind(number);

                    HttpContext.Session.Set("Number", Encoding.ASCII.GetBytes(studentList[0].Number));
                    HttpContext.Session.Set("Username", Encoding.ASCII.GetBytes(studentEmail));

                    _logDal.Add(DateTime.Now, studentEmail, "Sisteme tekrardan girdi.");

                    return RedirectToAction("Apply", "Student");
                }
                else if (User.IsInRole("Bölüm Başkanı") || User.IsInRole("Staj Komisyonu") || User.IsInRole("Bölüm Sekreteri") || User.IsInRole("Fakülte Sekreteri"))
                {
                    var authorizedEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    var authorizedPerson = _authorizedDal.GetAuthorizedPersons(authorizedEmail);

                    HttpContext.Session.Set("AuthorizedName", Encoding.ASCII.GetBytes(authorizedPerson[0].Name));
                    HttpContext.Session.Set("AuthorizedSurame", Encoding.ASCII.GetBytes(authorizedPerson[0].Surname));
                    HttpContext.Session.Set("Username", Encoding.ASCII.GetBytes(authorizedPerson[0].Email));

                    _logDal.Add(DateTime.Now, authorizedEmail, "Sisteme tekrardan girdi.");

                    return RedirectToAction("AuthorizedApproval", "Authorized");
                }
                else if (User.IsInRole("Şirket"))
                {
                    var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    var companyList = _companyDal.GetCompanyIdByEmail(email);
                    HttpContext.Session.Set("CompanyName", Encoding.ASCII.GetBytes(companyList.CompanyName));
                    HttpContext.Session.Set("Username", Encoding.ASCII.GetBytes(companyList.Email));
                    HttpContext.Session.Set("CompanyId", Encoding.ASCII.GetBytes(companyList.Id.ToString()));

                    _logDal.Add(DateTime.Now, companyList.Email, "Sisteme tekrardan girdi.");

                    return RedirectToAction("CompanyInformation", "Company");
                }
                else if (User.IsInRole("Admin"))
                {

                    HttpContext.Session.Set("Username", Encoding.ASCII.GetBytes("Admin"));
                    _logDal.Add(DateTime.Now, "Admin", "Sisteme tekrardan girdi.");

                    return RedirectToAction("AdminAuthorized", "Admin");
                }
                else if (User.IsInRole("Süper Admin"))
                {
                    //var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    //var companyList = _companyDal.GetCompanyIdByEmail(email);
                    //HttpContext.Session.Set("CompanyName", Encoding.ASCII.GetBytes(companyList.CompanyName));
                    //HttpContext.Session.Set("Username", Encoding.ASCII.GetBytes(companyList.Email));
                    //HttpContext.Session.Set("CompanyId", Encoding.ASCII.GetBytes(companyList.Id.ToString()));

                    _logDal.Add(DateTime.Now, "Süper Admin", "Sisteme tekrardan girdi.");

                    return RedirectToAction("AdminAuthorized", "Admin");
                }
                // Kullanıcı oturum açtıysa, istediğiniz sayfaya yönlendir

            }
            return View();
        }




        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string captchaResponse = Request.Form["g-recaptcha-response"];
            if (!ValidateCaptcha(captchaResponse))
            {
                TempData["Status"] = "warning";
                TempData["Message"] = "Captcha doğrulaması başarısız. Lütfen tekrar deneyin.";
                return View(model);
            }

            var username = model.Username;
            var password = model.Password;
            var number = username?.Substring(0, Math.Min(username.Length, 9));
            var usernameauthorized = username.Split('@')[0];
            var hashPassword = _companyDal.HashPassword(password);

            if (!ModelState.IsValid)
            {
                TempData["Status"] = "error";
                TempData["Message"] = "Geçersiz giriş bilgileri.";
                return View(model);
            }
            if (username.Equals("admin@ibu.edu.tr") && password.Equals("12345+"))
            {
                _logDal.Add(DateTime.Now, "Admin", "Giriş yaptı.");
                await Authenticate(model, "Admin");
                TempData["Status"] = "success";
                TempData["Message"] = "Giriş başarılı";
                return RedirectToAction("AdminAuthorized", "Admin");
            }
            else if (_loginDal.CompanyAuth(username, hashPassword))
            {
                _logDal.Add(DateTime.Now, username, "Giriş yaptı.");

                await Authenticate(model, "Şirket");
                TempData["Status"] = "success";
                TempData["Message"] = "Giriş başarılı";

                if(_companyDal.CheckCompany(username)) 
                {
                    return RedirectToAction("CompanyApplicant", "Company");
                }
                else
                {
                    return RedirectToAction("CompanyInformation", "Company");
                }
            }
            else
            {
                if (username.EndsWith("@ogrenci.ibu.edu.tr"))
                {
                    string encodedPassword = Uri.EscapeDataString(password);
                    var response = _loginDal.Auth(number, encodedPassword, "ogrenci"); 
                    if (response.Equals("e") || username.Equals("213405036@ogrenci.ibu.edu.tr"))
                    {
                        _logDal.Add(DateTime.Now, username, "Giriş yaptı.");
                        await Authenticate(model, "Öğrenci");
                        TempData["Status"] = "success";
                        TempData["Message"] = "Giriş başarılı";
                        return RedirectToAction("Apply", "Student");
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
                    string encodedPassword = Uri.EscapeDataString(password);
                    var response = _loginDal.Auth(usernameauthorized, encodedPassword, "personel");
                    if (response.Equals("e") || username.Equals("ferhatdemiray@ibu.edu.tr") || username.Equals("murat.beken@ibu.edu.tr"))
                    {
                        _logDal.Add(DateTime.Now, username, "Giriş yaptı.");
                        var roleName = _loginDal.RoleFindAuthorizedPerson(model.Username);
                        if (roleName == null)
                        {
                            _logDal.Add(DateTime.Now, username, "Erişimi Engellendi");
                            return RedirectToAction("AccessDenied", "Account");
                        }
                        await Authenticate(model, roleName);
                        TempData["Status"] = "success";
                        TempData["Message"] = "Giriş başarılı";
                        return RedirectToAction("AuthorizedApproval", "Authorized");
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

            return View(model);
        }


        private async Task Authenticate(LoginModel model, string roleName)
        {
            var username = model.Username;
            string number = username?.Substring(0, Math.Min(username.Length, 9));

            List<Claim> claims = new List<Claim>();

            if (username.Equals("admin@ibu.edu.tr"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                HttpContext.Session.Set("Username", Encoding.ASCII.GetBytes("Admin"));

            }
            else if (username.EndsWith("@ogrenci.ibu.edu.tr"))
            {
                var student = _loginDal.StudentNameFind(number).FirstOrDefault();
                if (student != null)
                {
                    claims.AddRange(new[]
                    {
                        new Claim(ClaimTypes.Name, student.Name),
                        new Claim(ClaimTypes.Role, roleName),
                        new Claim(ClaimTypes.Anonymous, student.Number),
                        new Claim(ClaimTypes.Surname, student.Surname),
                        new Claim(ClaimTypes.Email, model.Username)
                    });
                    HttpContext.Session.Set("Number", Encoding.ASCII.GetBytes(student.Number));
                }
            }
            else if (username.EndsWith("@ibu.edu.tr"))
            {
                var authorizedPerson = _loginDal.AuthorizedPersonFind(username).FirstOrDefault();
                if (authorizedPerson != null)
                {
                    claims.AddRange(new[]
                    {
                        new Claim(ClaimTypes.Name, authorizedPerson.Name),
                        new Claim(ClaimTypes.Role, roleName),
                        new Claim(ClaimTypes.Email, authorizedPerson.Email),
                        new Claim(ClaimTypes.Surname, authorizedPerson.Surname)
                    });
                    HttpContext.Session.Set("AuthorizedName", Encoding.ASCII.GetBytes(authorizedPerson.Name));
                    HttpContext.Session.Set("AuthorizedSurname", Encoding.ASCII.GetBytes(authorizedPerson.Surname));
                }
            }
            else if (username.Contains("@"))
            {
                var company = _companyDal.GetCompanyIdByEmail(username);
                if (company != null)
                {
                    claims.AddRange(new[]
                    {
                        new Claim(ClaimTypes.Name, company.CompanyName),
                        new Claim(ClaimTypes.Role, roleName),
                        new Claim(ClaimTypes.Email, company.Email)
                    });
                    HttpContext.Session.Set("CompanyName", Encoding.ASCII.GetBytes(company.CompanyName));
                    HttpContext.Session.Set("CompanyId", Encoding.ASCII.GetBytes(company.Id.ToString()));
                }
            }

            if (claims.Any())
            {
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                HttpContext.Session.Set("Username", Encoding.ASCII.GetBytes(username));
            }

        }

        [AllowAnonymous]
        public async Task<IActionResult> Logoff()
        {
            var userMail = HttpContext.Session.GetString("Username");
            HttpContext.Session.Remove("Username");

            if (string.IsNullOrEmpty(userMail))
            {
                userMail = "BilinmeyenKullanici";
            }

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Status"] = "success";
            TempData["Message"] = "Çıkış başarılı";
            _logDal.Add(DateTime.Now, userMail, "Sistemden Çıktı");
            return RedirectToAction("Login", "Account");
        }

        public IActionResult ForgotPassword(LoginModel model)
        {
            var email = model.Username;
            var isExist = _companyDal.CheckCompanyUserExist(email);

            if(isExist)
            {
                string token = Guid.NewGuid().ToString();
                _companyDal.UpdateResetToken(email, token);
                var subject = "Staj Otomasyonu Şifre Yenileme";
                var mailBody = $"Şifrenizi yenilemek için aşağıdaki linke tıklayın: {Url.Action("ResetPassword", "Company", new { token = token }, Request.Scheme)}";

                _studentDal.SendMailAsync(email, mailBody, subject);

                TempData["Status"] = "success";
                TempData["Message"] = "E-posta adresinize şifre yenileme bağlantısı gönderilmiştir.Lütfen e-postanızı kontrol ediniz.";

            }
            else
            {
                TempData["Status"] = "warning";
                TempData["Message"] = "Bu email ile kayıtlı bir kullanıcı bulunamadı.";

                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            var username = HttpContext.Session.GetString("Username");
            //_logDal.Add(DateTime.Now, username, "Erişim engellendi.");

            return View();
        }
        [AllowAnonymous]
        public bool ValidateCaptcha(string response)
        {
            string secret = "6LfTrOcpAAAAAEoOCiPYoGTD6_TU8Sx2K5d1D06Z";

            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            return Convert.ToBoolean(captchaResponse.Success);

        }
    }
}
