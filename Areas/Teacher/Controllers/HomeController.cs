using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QLDaoTao.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class HomeController : Controller
    {
        [Route("Teacher/Home")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
