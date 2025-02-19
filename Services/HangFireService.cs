using Hangfire;
using System.Linq.Expressions;

namespace QLDaoTao.Services
{
    public class HangFireService : IHangFire
    {
        public string GetJobState(string jobId)
        {
            var monitoringApi = JobStorage.Current.GetMonitoringApi();

            if (monitoringApi.SucceededJobs(0, 1000).Any(j => j.Key == jobId))
                return "Succeeded";

            if (monitoringApi.FailedJobs(0, 1000).Any(j => j.Key == jobId))
                return "Failed";

            if (monitoringApi.ProcessingJobs(0, 1000).Any(j => j.Key == jobId))
                return "Processing";

            if (monitoringApi.ScheduledJobs(0, 1000).Any(j => j.Key == jobId))
                return "Scheduled";

            if (monitoringApi.EnqueuedJobs("default", 0, 1000).Any(j => j.Key == jobId))
                return "Enqueued";

            return "Unknown (job not found)";
        }

        // Chạy ngay lập tức
        public string EnqueueJob(Expression<Action> method)
        {
            string resultId = BackgroundJob.Enqueue(method);
            return resultId;
        }

        // Chạy sau một khoảng thời gian
        public string ScheduleJob(Expression<Action> method, TimeSpan time)
        {
            string resultId = BackgroundJob.Schedule(method, time);
            return resultId;
        }

        // Chạy định kỳ
        public string RecurringJobb(Expression<Action> method, string cron)
        {
            string resultId = Guid.NewGuid().ToString();
            RecurringJob.AddOrUpdate(resultId, method, cron);
            return resultId;
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
                    foreach (var job in allJobState.EnqueuedJobs("default", 0, 1000))
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
