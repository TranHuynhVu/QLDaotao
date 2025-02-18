using Hangfire;
using System.Linq.Expressions;

namespace QLDaoTao.Services
{
    public class HangFireService : IHangFire
    {
        //Chạy ngay lập tức 
        public void EnqueueJob(Expression<Action> method)
        {
            BackgroundJob.Enqueue(method);
        }

        public void DequeueJob(Expression<Action> method)
        {
        }
        //Chạy sau một khoảng thời gian 
        public void ScheduleJob(Expression<Action> method, TimeSpan time)
        {
            BackgroundJob.Schedule(method, time);
        }

        //Chạy định kỳ 
        public void RecurringJobb(Expression<Action> method, string cron)
        {
            RecurringJob.AddOrUpdate(Guid.NewGuid().ToString(), method, cron);

        }
        // Lấy danh sách các công việc theo trạng thái
        public List<string> GetJobByState(string state)
        {
            // Danh sách jobs với tất cả trạng thái
            var allJobState = JobStorage.Current.GetMonitoringApi();
            var Jobs = new List<string>();
            switch (state)
            {
                case "enqueued":
                    foreach(var job in allJobState.EnqueuedJobs("default", 0, 1000))
                    {
                        Jobs.Add($"ID: {job.Key}, Trạng thái: Enqueued, Phương thức: {job.Value.Job.Method.Name}");
                    }
                    break;
                case "scheduled":
                    foreach (var job in allJobState.ScheduledJobs(0, 1000))
                    {
                        Jobs.Add($"ID: {job.Key}, Trạng thái: Scheduled, Phương thức: {job.Value.Job.Method.Name}");
                    }
                    break;

                case "processing":
                    foreach (var job in allJobState.ProcessingJobs(0, 1000))
                    {
                        Jobs.Add($"ID: {job.Key}, Trạng thái: Processing, Phương thức: {job.Value.Job.Method.Name}");
                    }
                    break;

                case "succeeded":
                    foreach (var job in allJobState.SucceededJobs(0, 1000))
                    {
                        Jobs.Add($"ID: {job.Key}, Trạng thái: Succeeded, Hoàn thành lúc: {job.Value.SucceededAt}");
                    }
                    break;

                case "failed":
                    foreach (var job in allJobState.FailedJobs(0, 1000))
                    {
                        Jobs.Add($"ID: {job.Key}, Trạng thái: Failed, Lỗi: {job.Value.ExceptionMessage}");
                    }
                    break;

                default:
                    throw new ArgumentException("Trạng thái không hợp lệ. Hãy chọn: enqueued, scheduled, processing, succeeded, failed.");
            }
            return Jobs;
        }
    }
}
