using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using QLDaoTao.Data;
using QLDaoTao.Models;
using QLDaoTao.ViewModels;

namespace QLDaoTao.Services
{
    public class ItemNotificationService : INotification
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _notifHub;
        public ItemNotificationService(AppDbContext context, IHubContext<NotificationHub> notifHub)
        {
            _context = context;
            _notifHub = notifHub;
        }

        public async Task<bool> SendNotiByTeacher(NotificationVM notiVm, string magv)
        {
            try
            {
                await _notifHub.Clients.Group(magv.ToString()).SendAsync("ReceiveNotiTeacher", notiVm, magv.ToString());
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<NotificationVM> CreateNoti(string title, string description, string receiver, string typeNoti, int idPhieu)
        {
            Notification noti = new Notification
            {
                Title = title,
                Description = description,
                Receiver = receiver,
                CreatedAt = DateTime.Now,
                Status = 0,
                TypeNoti = typeNoti,
                IdPhieu = idPhieu
            };
            await _context.Notifications.AddAsync(noti);
            await _context.SaveChangesAsync();
            var notiCount = await GetStatusCount(noti.Receiver);
            return new NotificationVM
            {
                Id = noti.Id,
                Title = noti.Title,
                Description = noti.Description,
                Receiver = noti.Receiver,
                CreatedAt = noti.CreatedAt,
                Status = noti.Status,
                CountStatus = notiCount,
                IdPhieu = noti.IdPhieu
            };
        }

        public async Task<int> GetStatusCount(string magv)
        {
            return await _context.Notifications
                        .Where(n => n.Receiver == magv && n.TypeNoti == "Teacher" && n.Status == 0)
                        .CountAsync();
        }
    }
}
