namespace QLDaoTao.Areas.Teacher.Services
{
    public interface INotificationTeacher
    {
        Task<int> GetStatusCount(string magv);
    }
}
