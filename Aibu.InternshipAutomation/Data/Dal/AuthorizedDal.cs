using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Context;
using Aibu.InternshipAutomation.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aibu.InternshipAutomation.Data.Dal
{
    public class AuthorizedDal : IAuthorizedDal
    {
        private readonly DatabaseContext _databaseContext;

        public AuthorizedDal(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public List<AuthorizedViews> GetAll()
        {
            return _databaseContext.AuthorizedView.ToList();
        }
        public List<AuthorizedViews> GetAll(string number)
        {
            var studentList = _databaseContext.Student.Where(p => p.Number.Contains(number)).Select(s => new AuthorizedViews
            {
                AcceptanceStatus = s.AcceptanceStatusId
            }).ToList();
            return studentList;
        }
        public List<AuthorizedViews> CheckStudentStatus(string email, int status)
        {
            var authorizedPerson = _databaseContext.AuthorizedPerson.FirstOrDefault(p => p.Email.Equals(email));

            var departmentIds = _databaseContext.AuthorizedDepartments.Where(p => p.AuthorizedPersonId == authorizedPerson.Id).Select(p => p.DepartmentId).ToList();
            var studentList = _databaseContext.AuthorizedView.Where(p => departmentIds.Contains(p.DepartmentId) && p.RoleId == authorizedPerson.RoleId && p.AcceptanceStatus == status).ToList();

            return studentList;
        }
        public List<AuthorizedPersons> GetAuthorizedPersons(string email)
        {
            List<AuthorizedPersons> authorizedPersonList = new List<AuthorizedPersons>();
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("E-posta null veya boş olamaz.", nameof(email));
            }
            else
            {
                authorizedPersonList = _databaseContext.AuthorizedPerson.Where(p => p.Email.Contains(email)).ToList();
                return authorizedPersonList;
            }
        }

        public string RoleFindAuthorizedPerson(string email)
        {
            var role = _databaseContext.RoleView.FirstOrDefault(p => p.Email.Equals(email));
            if(role is null)
            {
                return null;
            }
            else
            {
                return role.RoleName;
            }         
        }

        public int RoleIdFindAuthorizedPerson(string email)
        {
            var role = _databaseContext.AuthorizedPerson.FirstOrDefault(p => p.Email.Equals(email));

            return (int)role.RoleId;
        }

        public string RoleFindAuthorizedPersonPdf(string email)
        {

            var role = _databaseContext.RoleView.FirstOrDefault(p => p.Email.Equals(email));
            if(role  == null)
            {
                return null;
            }

            return role.RoleName;
        }

        public List<AuthorizedPersonDepartmentViews> GetAllPerson(int roleId, int deparmentId)
        {
            var list = _databaseContext.AuthorizedPersonDepartmentView.Where(p => p.RoleId == roleId && p.DepartmentId == deparmentId).ToList();
            return list;
        }

        public List<AuthorizedViews> AuthorizedApproved(int roleId, string email)
        {
            List<AuthorizedViews> studentList = new List<AuthorizedViews>();
            var authorizedPerson = _databaseContext.AuthorizedPerson.FirstOrDefault(p => p.Email.Equals(email));
            var departmentIds = _databaseContext.AuthorizedDepartments.Where(p => p.AuthorizedPersonId == authorizedPerson.Id ).Select(p => p.DepartmentId).ToList();

            studentList= _databaseContext.AuthorizedView.Where(p => departmentIds.Contains(p.DepartmentId) && p.RoleId > roleId && p.AcceptanceStatus == 1).OrderByDescending(p => p.UpdateTime).ToList();

            return studentList;
        }
    }
}
