using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using QLDaoTao.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QLDaoTao.Models
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly UserManager<AppUser> _userManager;

        public NotificationHub(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _userManager.GetUserAsync(Context.User);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
            }
            if (roles.Contains("Teacher"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, user.UserName);
            }
            if (user.Role == "Student")
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, user.UserName);
            }
            await base.OnConnectedAsync();
        }

        public async Task SendNotificationToAdmins(Notification notification, int notificationCount)
        {
            await Clients.Group("Admin").SendAsync("ReceiveNotiAdmin", notification, notificationCount);
        }

        public async Task SendNotificationToStudents(NotificationVM notificationVM, int notificationCount, string masv)
        {
            await Clients.User(masv).SendAsync("ReceiveNotiStudent", notificationVM, notificationCount);
        }
        public async Task SendNotificationToTeacher(NotificationVM notificationVM, string magv)
        {
            await Clients.User(magv).SendAsync("ReceiveNotiTeacher", notificationVM);
        }

    }
}
