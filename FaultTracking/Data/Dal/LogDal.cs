using FaultTracking.Data.Base;
using FaultTracking.Data.Context;

namespace FaultTracking.Data.Dal
{
    public class LogDal : ILogDal
    {
        private readonly DatabaseContext _databaseContext;

        public LogDal(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public Logs? Add(DateTime createTime, string createUser, string message)
        {

            var log = new Logs()
            {
                CreateTime = createTime,
                CreateUser = createUser,
                Message = message,

            };

            var entity = _databaseContext.Log.Add(log);
            _databaseContext.SaveChanges();
            return entity.Entity;
        }
    }
}
