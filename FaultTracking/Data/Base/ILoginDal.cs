using FaultTracking.Data.Entities;

namespace FaultTracking.Data.Base
{
    public interface ILoginDal
    {
        public string Auth(string username, string password, string tur);

        public string RoleFindAuthorizedPerson(string username);
        public List<Ubyss> StudentNameFind(string number);

        public List<AuthorizedUbyss> AuthorizedPersonFind(string username);
    }
}
