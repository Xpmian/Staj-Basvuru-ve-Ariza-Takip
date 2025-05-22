using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Context;
using Aibu.InternshipAutomation.Data.Entities;

namespace Aibu.InternshipAutomation.Data.Dal
{
    public class LoginDal : ILoginDal
    {
        private DatabaseContext _databaseContext;

        public LoginDal(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public bool Auth(string username, string password)
        {
            Userss p = _databaseContext.Users.Where(p => p.Username.Equals(username) && p.Password.Equals(password)).SingleOrDefault();

            if (p != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string RoleFind(string username, string password)
        {
            List<RoleViews> list2 = new List<RoleViews>();
            list2 = _databaseContext.RoleView.Where(p => p.Username.Equals(username) && p.Password.Equals(password)).ToList();
            var a = list2[0].RoleName;

            return a.ToString();

        }

        //public List<UserView> GetAll()
        //{
        //    return _databaseContext.UserViews.ToList();
        //}

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
    }
}
