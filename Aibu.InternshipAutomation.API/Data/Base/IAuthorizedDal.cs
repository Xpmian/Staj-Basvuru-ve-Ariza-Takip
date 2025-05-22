using Aibu.InternshipAutomation.Data.Entities;

namespace Aibu.InternshipAutomation.Data.Base
{
    public interface IAuthorizedDal
    {
        public List<AuthorizedViews> GetAll();
        public List<AuthorizedViews> StudentRejected(string email);
        public List<AuthorizedViews> StudentAwaitingApproval(string email);
    }
}
