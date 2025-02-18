using System.Linq.Expressions;

namespace QLDaoTao.Services
{
    public interface IHangFire
    {
        string GetJobState(string jobId);
        string EnqueueJob(Expression<Action> method);
        string ScheduleJob(Expression<Action> method, TimeSpan time);
        string RecurringJobb(Expression<Action> method, string cron);
        List<string> GetJobByState(string state);
    }
}
