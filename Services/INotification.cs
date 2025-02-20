using QLDaoTao.ViewModels;

namespace QLDaoTao.Services
{
    public interface INotification
    {
        Task<int> GetStatusCount(string magv);
        Task<NotificationVM> CreateNoti(string title, string description, string receiver, string typeNoti);
        Task<bool> SendNotiByTeacher(NotificationVM notiVm, string magv);
    }
}
