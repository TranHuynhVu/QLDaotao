using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QLDaoTao.Areas.Admin.Services;
using QLDaoTao.Models;

namespace QLDaoTao.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class LopHocPhanController : Controller
    {
        private readonly ILopHocPhan _lopHocPhan;
        private readonly UserManager<AppUser> _userManager;

        public LopHocPhanController(ILopHocPhan lopHocPhan, UserManager<AppUser> userManager)
        {
            _lopHocPhan = lopHocPhan;
            _userManager = userManager;
        }
        [Route("Teacher/LopHocPhan/ListByTeacher/")]
        public async Task<IActionResult> ListByTeacher()
        {
            var user = await _userManager.GetUserAsync(User);
            var list = await _lopHocPhan.ListByTeacher(int.Parse(user.UserName));
            return Json(new {Data = list});
        }
    }
}
