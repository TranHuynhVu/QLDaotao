using System.Linq.Expressions;

namespace QLDaoTao.Services
{
    public interface IHangFire
    {
        void EnqueueJob(Expression<Action> method);
        void ScheduleJob(Expression<Action> method, TimeSpan time);
        void RecurringJobb(Expression<Action> method, string cron);
        List<string> GetJobByState(string state);
    }
}
