using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QLDaoTao.Models;
using QLDaoTao.Services;


namespace QLDaoTao.Controllers
{
    public class HangFireController : Controller
    {
        private readonly IHangFire _hangFire;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly UserManager<AppUser> _userManager;

        public HangFireController(IHangFire hangFire, IHubContext<NotificationHub> notificationHub, UserManager<AppUser> userManager)
        {
            _hangFire = hangFire;
            _notificationHub = notificationHub;
            _userManager = userManager;
        }
/*
        [Route("/sendNoti")]
        public async Task<IActionResult> SendNotifications()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.Role == "Student")
            {
                //_hangFire.RecurringJobb(() => SendNoti(user.UserName), "* * * * *");
                _hangFire.EnqueueJob(() => SendNoti(user.UserName));
            }
            return View();
        }
       
        public void SendNoti(string masv)
        {
            var noti = new NotificationVM
            {
                Id = 1,
                Name = "Thông báo",
                Title = "Gửi thông báo thành công",
                NoiDung = "Bạn có một thông báo mới",
                NgayTao = DateTime.Now,
                Status = 0
            };

            // Gửi thông báo dựa trên danh sách kết nối trong NotificationHub
             _notificationHub.Clients.Group(masv).SendAsync("ReceiveNotiStudent", noti, 1, masv);
        }
*/
    }
}
