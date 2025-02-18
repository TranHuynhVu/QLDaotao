using QLDaoTao.Models;
using QLDaoTao.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace QLDaoTao.Controllers;

public class AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager) : Controller
{
    [Route("/login")]
    [Route("/Account/Login")]
   
    public async Task<IActionResult> Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await userManager.GetUserAsync(User);
            var roles = await userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else
            {
                if (user.Role == "Teacher")
                    return RedirectToAction("Index", "Home", new { area = "Teacher" });
                else if (user.Role == "Student")
                    return RedirectToAction("SinhVien", "Home");
                else
                    return RedirectToAction("index", "home");
            }
        }
        return View();
    }

    [HttpPost]
    [Route("/login")]
    [Route("/Account/Login")]
    public async Task<IActionResult> Login(LoginVM model)
    {
        if (ModelState.IsValid)
        {
            var username = model.Username.Trim().ToUpperInvariant();
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                if (user.Status == 1)
                {
                    //login
                    var result = await signInManager.PasswordSignInAsync(username!, model.Password!, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        var roles = await userManager.GetRolesAsync(user);
                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                        }
                        else
                        {
                            if (roles.Contains("Teacher"))
                                return RedirectToAction("Index", "Home", new { area = "Teacher" });
                            else if (user.Role == "Student")
                                return RedirectToAction("SinhVien", "Home");
                            else
                                return RedirectToAction("index", "home");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng nhập thất bại!");
                    }

                }else
                {
                    ModelState.AddModelError("", "Tài khoản chưa được kích hoạt");
                }
            }else
            {
                ModelState.AddModelError("", "Tài khoản không tồn tại!");
            }
            return View(model);
        }
        return View(model);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM model)
    {
        if (ModelState.IsValid)
        {
            
            AppUser user = new AppUser();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Name = model.LastName.Trim() + ' ' + model.FirstName;
            user.UserName = model.UserCode.Trim();
            user.Email = model.Email.Trim();
            user.NormalizedEmail = model.Email.Trim().ToUpperInvariant();
            user.Password = model.Password;
            user.Address = model.Address;
            user.Status = 1;
            user.Role = "Admin";
            var result = await userManager.CreateAsync(user, model.Password!);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }
    [Route("/dang-xuat")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index","Home");
    }
    [HttpGet]
    [AllowAnonymous]
    [Route("opps")]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [Route("/admin")]
    [Route("/Account/AdminLogin")]

    public async Task<IActionResult> AdminLogin()
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await userManager.GetUserAsync(User);
            var roles = await userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else
            {
                if (user.Role == "Teacher")
                    return RedirectToAction("Index", "Home", new { area = "Teacher" });
                else if (user.Role == "Student")
                    return RedirectToAction("SinhVien", "Home");
                else
                    return RedirectToAction("index", "home");
            }
        }
        return View();
    }
    [HttpPost]
    [Route("/admin")]
    [Route("/Account/AdminLogin")]
    public async Task<IActionResult> AdminLogin(LoginVM model)
    {
        if (ModelState.IsValid)
        {
            var username = model.Username.Trim();
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                if (user.Status == 1)
                {
                    //login
                    var result = await signInManager.PasswordSignInAsync(username!, model.Password!, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        var roles = await userManager.GetRolesAsync(user);
                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                        }
                        else
                        {
                            if (user.Role == "Teacher")
                                return RedirectToAction("Index", "Home", new { area = "Teacher" });
                            else if (user.Role == "Student")
                                return RedirectToAction("SinhVien", "Home");
                            else
                                return RedirectToAction("index", "home");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng nhập thất bại!");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản chưa được kích hoạt");
                }
            }
            else
            {
                ModelState.AddModelError("", "Tài khoản không tồn tại!");
            }
            return View(model);
        }
        return View(model);
    }
}
