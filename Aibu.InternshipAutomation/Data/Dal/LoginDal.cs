using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Context;
using Aibu.InternshipAutomation.Data.Entities;
using Aibu.InternshipAutomation.Helper;
using Argon;

namespace Aibu.InternshipAutomation.Data.Dal
{
    public class LoginDal : ILoginDal
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILdapService _ldapService;

        public LoginDal(DatabaseContext databaseContext, ILdapService ldapService)
        {
            _databaseContext = databaseContext;
            _ldapService = ldapService;

        }

        public string Auth(string username, string password, string tur)
        {
            var a = _ldapService.AuthenticateStudentWithLdap(username, password, tur);
            JObject obj = JObject.Parse(a);
            string harf = (string)obj["sonuc"];
            return harf;
        }

        public bool CompanyAuth(string username, string password)
        {
            var companyUsers = _databaseContext.CompanyUsers.FirstOrDefault(p => p.Email.Equals(username) && p.Password.Equals(password));
            return companyUsers != null;
        }

        public string RoleFindAuthorizedPerson(string email)
        {

            var role = _databaseContext.RoleView.FirstOrDefault(p => p.Email.Equals(email));
            if (role is null)
                return null;

            return role.RoleName;

        }

        public string GetBeforeAtSymbol(string email)
        {
            // E-posta adresinde @ simgesinin index'ini bul
            int atIndex = email.IndexOf('@');

            // @ simgesi bulunamazsa veya ilk karakterde ise boş bir dize döndür
            if (atIndex <= 0)
            {
                return string.Empty;
            }

            // @ simgesinden önceki kısmı al
            string beforeAtSymbol = email.Substring(0, atIndex);

            return beforeAtSymbol;
        }
        public List<Ubyss> StudentNameFind(string number)
        {
            return _databaseContext.Ubys.Where(p => p.Number.Equals(number)).ToList();
        }

        public List<AuthorizedUbyss> AuthorizedPersonFind(string username)
        {

            return _databaseContext.AuthorizedUbys.Where(p => p.Email.Equals(username)).ToList();

        }
    }
}
