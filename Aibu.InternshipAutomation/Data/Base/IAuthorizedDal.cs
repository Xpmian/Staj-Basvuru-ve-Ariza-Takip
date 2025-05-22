using Aibu.InternshipAutomation.Data.Entities;

namespace Aibu.InternshipAutomation.Data.Base
{
    public interface IAuthorizedDal
    {
        public List<AuthorizedViews> GetAll();
        List<AuthorizedViews> CheckStudentStatus(string email, int status);
        public List<AuthorizedPersons> GetAuthorizedPersons(string email);
        public List<AuthorizedViews> GetAll(string number);
        public string RoleFindAuthorizedPerson(string email);
        public int RoleIdFindAuthorizedPerson(string email);
        public List<AuthorizedPersonDepartmentViews> GetAllPerson(int roleId, int deparmentId);
        public string RoleFindAuthorizedPersonPdf(string email);
        public List<AuthorizedViews> AuthorizedApproved(int roleId, string email);
    }
}
