using Microsoft.EntityFrameworkCore;
using QLDaoTao.Data;

namespace QLDaoTao.Areas.Teacher.Services
{
    public class NotificationTeacherService : INotificationTeacher
    {
        private readonly AppDbContext _context;
        public NotificationTeacherService(AppDbContext context)
        {
            _context = context;
        }
        public async int Task<GetStatusCount>(string magv) { 
            return await _context.Notifications
                        .Where(n => n.Receiver == magv && n.TypeNoti == "Teacher" && n.Status == 0)
                        .CountAsync(); 
        }
    }
}
