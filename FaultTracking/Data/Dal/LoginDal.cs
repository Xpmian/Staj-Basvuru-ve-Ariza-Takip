using FaultTracking.Data.Base;
using FaultTracking.Data.Context;
using FaultTracking.Data.Entities;
using FaultTracking.Helper;
using Newtonsoft.Json.Linq;

namespace FaultTracking.Data.Dal
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

        public string RoleFindAuthorizedPerson(string email)
        {
            var role = _databaseContext.RoleView.FirstOrDefault(p => p.Email.Equals(email));
            if (role == null)
            {
                return "Diger";
            }

            return role.Role;
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
