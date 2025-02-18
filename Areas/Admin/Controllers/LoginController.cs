using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QLDaoTao.Models;

namespace QLDaoTao.Areas.Admin.Controllers
{
    public class LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager) : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}
