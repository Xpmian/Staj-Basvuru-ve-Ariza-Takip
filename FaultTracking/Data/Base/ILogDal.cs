using FaultTracking.Data.Dal;

namespace FaultTracking.Data.Base
{
    public interface ILogDal
    {
        public Logs? Add(DateTime createTime, string createUser, string message);
    }
}
