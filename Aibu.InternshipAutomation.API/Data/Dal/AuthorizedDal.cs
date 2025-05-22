using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Context;
using Aibu.InternshipAutomation.Data.Entities;

namespace Aibu.InternshipAutomation.Data.Dal
{
    public class AuthorizedDal :IAuthorizedDal
    {
        private readonly DatabaseContext _databaseContext;
        private readonly StudentDal _studentDal;

        public AuthorizedDal(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public List<AuthorizedViews> GetAll()
        {
            return _databaseContext.AuthorizedView.ToList();
        }
        public List<AuthorizedViews> StudentRejected(string email)
        {
            var a = _databaseContext.AuthorizedPerson.FirstOrDefault(p =>p.Email.Contains(email));
            var b = _databaseContext.AuthorizedView.Where(p => p.DepartmentId == a.DepartmentId);
            List<AuthorizedViews> list = new List<AuthorizedViews>();

            foreach (var item in b)
            {
                int temp = item.AcceptanceStatus;
                int temp2 = item.RoleId;
                if (temp == 0 && temp2 == a.RoleId)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public List<AuthorizedViews> StudentAwaitingApproval(string email)
        {
            var a = _databaseContext.AuthorizedPerson.FirstOrDefault(p => p.Email.Contains(email));
            var b = _databaseContext.AuthorizedView.Where(p => p.DepartmentId == a.DepartmentId);
            List<AuthorizedViews> list = new List<AuthorizedViews>();

            foreach (var item in b)
            {
                int temp = item.AcceptanceStatus;
                int temp2 = item.RoleId;
                if (temp == 1 && temp2 == a.RoleId)
                {
                    list.Add(item);
                }
            }
            return list;
        }
    }
}
