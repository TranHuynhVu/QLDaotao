using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLDaoTao.Data;
using QLDaoTao.Models;
using QLDaoTao.ViewModels;

namespace QLDaoTao.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class NotificationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public NotificationController(AppDbContext context, UserManager<AppUser> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var notis = await _context.Notifications
                                .Where(n => n.Receiver == user.UserName)
                                .OrderByDescending(o => o.CreatedAt)
                                .ToListAsync();
            var SetNotiStatus = await _context.Notifications
                            .Where(n => n.Receiver == user.UserName && n.Status == 0)
                            .ToListAsync();
            foreach (var item in SetNotiStatus)
            {
                item.Status = 1;
            }
            await _context.SaveChangesAsync();

            return View(notis);
        }
    }
}
