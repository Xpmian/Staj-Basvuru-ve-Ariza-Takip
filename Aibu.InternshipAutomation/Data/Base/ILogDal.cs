using Aibu.InternshipAutomation.Data.Entities;

namespace Aibu.InternshipAutomation.Data.Base
{
    public interface ILogDal
    {

        public Logs? Add(DateTime createTime, string createUser, string message);
    }
}
