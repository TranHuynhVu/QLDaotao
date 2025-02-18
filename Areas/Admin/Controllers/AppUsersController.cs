using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLDaoTao.Data;
using QLDaoTao.Data.Enums;
using QLDaoTao.Models;
using QLDaoTao.Areas.Admin.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using QLDaoTao.Areas.Admin.Data;
using System.Security.Policy;
using System.Data;
using NuGet.DependencyResolver;


namespace QLDaoTao.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AppUsersController : Controller
    {
        private readonly AppDbContext _context;
        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private readonly IWebHostEnvironment _hostingEnviroment;
        private readonly RoleManager<IdentityRole> roleManager;
        public AppUsersController(
            UserManager<AppUser> usrMgr,
            IPasswordHasher<AppUser> passwordHash,
            AppDbContext context,
            IWebHostEnvironment hostingEnviroment,
            RoleManager<IdentityRole> roleMgr
            )
        {
            userManager = usrMgr;
            _context = context;
            passwordHasher = passwordHash;
            _hostingEnviroment = hostingEnviroment;
            roleManager = roleMgr;
        }

        // GET: Admin/AppUsers
        [Route("admin/users")]
        public async Task<IActionResult> Index()

        {
            //var appUsers = await _context.AppUsers.Where(u => u.Role != "Teacher" && u.Role != "Student").ToListAsync();
            //var data = appUsers;
            return View();
        }
        [Route("admin/users/list")]
        public async Task<IActionResult> getList()
        {
            var appUsers = await _context.AppUsers.Where(u => u.Role != "Teacher" && u.Role != "Student" && u.Status == 1).ToListAsync();
            return Json(new
            {
                data = appUsers
            });
        }
        public ViewResult Create() => View();
        [HttpPost]

        public async Task<IActionResult> Create(UserVM user)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Name = user.LastName + " " + user.FirstName,
                    UserName = user.Email,
                    Email = user.Email,
                    NormalizedEmail = user.Email.ToUpper(),
                    Phone = user.Phone,
                    //Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    Status = 1,
                    Address = user.Address,
                    Role = "Admin",
                    Tel = user.Tel
                };
                if (user.IUrlAvatar != null)
                {
                    string uniqueFileName = UploadedFile(user.IUrlAvatar);
                    appUser.UrlAvatar = "/Uploads/Users/" + uniqueFileName;
                }
                else
                {
                    appUser.UrlAvatar = "/Uploads/Users/default.jpg";
                }
                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);

                if (result.Succeeded)
                {
                    TempData["msg"] = "Thêm mới thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }
        // Chỉnh sửa thông tin Users
        //[Authorize(Roles = "Student")]
        [Route("admin/user/edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditUser(System.String id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            TempData["isCheckbox"] = appUser.Status == 1 ? "checked" : "";
            if (appUser == null)
            {
                return NotFound();
            }
            //TempData["AppUserData"] = appUser;
            return View(appUser);
        }


        [Route("Admin/AppUsers/Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserVM model)
        {
            var user = await userManager.FindByIdAsync(model.Id.ToString());

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Name = user.LastName + " " + user.FirstName;
                user.UserName = model.Email;
                user.Email = model.Email;
                user.NormalizedEmail = model.Email.ToUpper();
                user.Phone = model.Phone;
                //user.Gender = model.Gender;
                user.DateOfBirth = model.DateOfBirth;
                user.Status = model.Status != null ? 1 : 0;
                user.Address = model.Address;
                //user.Role = model.Role;
                user.Tel = model.Tel;
                if (model.IUrlAvatar != null)
                {
                    string uniqueFileName = UploadedFile(model.IUrlAvatar);
                    user.UrlAvatar = "/Uploads/Users/" + uniqueFileName;
                }
                else
                {
                    user.UrlAvatar = "/Uploads/Users/default.jpg";
                }
                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["msg"] = "Cập nhật thành công!";
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View(model);
        }
        [HttpDelete]
        [Route("admin/user/delete/{id}")]
        public async Task<IActionResult> Delete(System.String id)
        {

            var user = await userManager.FindByIdAsync(id.ToString());
            //var roles = await userManager.GetRolesAsync(user);
            //if (roles.Contains("Admin") || roles.Contains("HeadOfDepartment"))
            //{
            //    return RedirectToAction("ListUsers");
            //}
            //else
            //{
            //    var result = await userManager.DeleteAsync(user);
            //    TempData["msg"] = "Xóa nhân sự thành công.";
            //    return RedirectToAction("ListUsers");
            //}

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Json(new { success = true, message = "Xóa người dùng thành công" });

            }
            else
            {
                return Json(new { success = true, message = "Xóa người dùng thất bại" });
            }

            //TempData["msg"] = "Xóa người dùng thành công thành công.";
            //return RedirectToAction("Index");
        }

        public string UploadedFile(IFormFile IUrlAvatar)
        {
            string uniqueFileName = null;
            if (IUrlAvatar != null)
            {
                int len = IUrlAvatar.FileName.Length - 4;// Trừ cho 4 vì kiểm tra đuôi file
                string fileTag = IUrlAvatar.FileName.Substring(len);
                string[] acceptedExtensions = { ".jpg", ".jpeg", ".png", ".PNG",".JPG", ".JPEG" };

                if (acceptedExtensions.Contains(Path.GetExtension(IUrlAvatar.FileName).ToLower()))
                {
                    string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "Uploads/Users");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + IUrlAvatar.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        IUrlAvatar.CopyTo(fileStream);
                    }
                }
            }
            return uniqueFileName;
        }
        [Route("admin/manager-user-roles/{id}")]
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(String id)
        {
            ViewBag.userId = id;

            var user = await userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesVM>();
            var roles = roleManager.Roles.ToList();
            foreach (var role in roles)
            {
                var userRolesViewModel = new UserRolesVM
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                var nameRole = role.Name;

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }


        [Route("admin/manager-user-roles/{userId}")]
        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesVM> model, String userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("Index");
        }
        [Route("admin/user/detail/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(String id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [Route("admin/user/reset-password/{id}")]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            ViewBag.FullName = user.Name;
            string token = TokenGenerator.GenerateToken();
            var model = new ResetPasswordModel { Token = token, Id = id };
            return View(model);
        }
        [Route("admin/user/reset-password/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);

            var user = await userManager.FindByIdAsync(resetPasswordModel.Id);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));
            var hasher = new PasswordHasher<ResetPasswordModel>();
            user.PasswordHash = hasher.HashPassword(null, resetPasswordModel.Password);
            var result = await userManager.UpdateAsync(user);
            //var resetPassResult = await userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (result.Succeeded)
            {
                TempData["msg"] = "Đặt lại mật khẩu thành công!";
                return RedirectToAction("Index");
            }

            return View(resetPasswordModel);
        }

        //[HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [Route("admin/updatePasswordHashTeacher")]
        [HttpGet]
        public async Task UpdateTeacherPasswordHashAsync()
        {
            var teachers = await _context.Teachers
             .Select(t => new AppTeacher
             {
                 Id = t.Id,
                 PasswordHash = t.PasswordHash,
                 Password = t.Password
             }).ToListAsync();

            foreach (var teacher in teachers)
            {
                var hasher = new PasswordHasher<ResetPasswordModel>();
                teacher.PasswordHash = hasher.HashPassword(null, teacher.Password);
            }

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }
        [Route("admin/updatePasswordHashStudent")]
        [HttpGet]
        public async Task UpdatePasswordsAsync()
        {
            var students = await _context.Students.ToListAsync();

            foreach (var student in students)
            {
                // Kiểm tra xem cột hashPass có giá trị hay không
                if (!string.IsNullOrEmpty(student.Password))
                {
                    // Cập nhật password hash từ hashPass vào cột PasswordHash
                    var hasher = new PasswordHasher<ResetPasswordModel>();
                    student.PasswordHash = hasher.HashPassword(null, student.Password);

                    // Đặt hashPass về null để tránh sử dụng nó sau này
                }
            }

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }

    }
}
