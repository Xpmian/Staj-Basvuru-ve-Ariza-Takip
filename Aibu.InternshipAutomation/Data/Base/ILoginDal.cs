using Aibu.InternshipAutomation.Data.Entities;

namespace Aibu.InternshipAutomation.Data.Base
{
    public interface ILoginDal
    {
        public string GetBeforeAtSymbol(string email);

        public string Auth(string username, string password, string tur);
        public bool CompanyAuth(string username, string password);

        public string RoleFindAuthorizedPerson(string username);
        public List<Ubyss> StudentNameFind(string number);

        public List<AuthorizedUbyss> AuthorizedPersonFind(string username);
    }
}
