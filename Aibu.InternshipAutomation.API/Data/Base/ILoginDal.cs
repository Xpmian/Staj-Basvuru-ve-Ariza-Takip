namespace Aibu.InternshipAutomation.Data.Base
{
    public interface ILoginDal
    {
        //List<UserView> GetAll();
        public string GetBeforeAtSymbol(string email);

        public bool Auth(string username, string password);

        public string RoleFind(string username, string password);
    }
}
