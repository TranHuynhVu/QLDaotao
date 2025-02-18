using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using QLDaoTao.Models;

namespace QLDaoTao.Areas.Student.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private UserManager<AppStudent> userManager;
        private SignInManager<AppStudent> signInManager;

        public StudentController(UserManager<AppStudent> userMgr, SignInManager<AppStudent> signinMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login();
            return View("~/Areas/Student/Views/Login/Login.cshtml");
        }

    
    }
}