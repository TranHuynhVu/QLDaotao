using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QLDaoTao.Models;
using QLDaoTao.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using QLDaoTao.Data;
namespace QLDaoTao.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AppRolesController : Controller
    {
        private readonly AppDbContext _context;
        private UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AppRolesController(
            UserManager<AppUser> usrMgr,
            AppDbContext context, RoleManager<IdentityRole> roleMgr)
        {
            roleManager = roleMgr;
            userManager = usrMgr;
            _context = context;
        }
        [Route("admin/roles")]
        [HttpGet]
        public IActionResult Index()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }
        [Route("admin/role/create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Route("admin/role/create")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleVM model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole appRole = new IdentityRole
                {
                    Name = model.Name
                };
                IdentityResult result = await roleManager.CreateAsync(appRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // Chỉnh sửa các Roles
        [Route("admin/role/edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(String id)
        {
            // Find the role by Role ID
            var role = await roleManager.FindByIdAsync(id.ToString());

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleVM
            {
                Id = role.Id,
                Name = role.Name
            };
            var appUsers = await _context.AppUsers.Where(u => u.Role != "Teacher" && u.Role != "Student" && u.Status == 1).ToListAsync();
            foreach (var user in appUsers)
            {

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [Route("admin/role/edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleVM model)
        {
            var role = await roleManager.FindByIdAsync(model.Id.ToString());
            if (role == null)
            {
                ViewBag.ErrorMessage =
                    $"Role with Id: {model.Id} could not be found";
                return View("NotFound");
            }

            role.Name = model.Name;

            var result = await roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        // Thêm xóa User Roles
        [Route("admin/edit-user-role/{roleId}")]
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(String roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId.ToString());
            ViewBag.RoleName = role.Name;
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleVM>();
            var appUsers = await _context.AppUsers.Where(u => u.Role != "Teacher" && u.Role != "Student" && u.Status == 1).ToListAsync();
            foreach (var user in appUsers)
            {
                var userRoleViewModel = new UserRoleVM
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FullName = user.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }
        [Route("admin/edit-user-role/{roleId}")]
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleVM> model, String roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId.ToString());

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId.ToString());

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("Edit", new { Id = roleId });
                }
            }

            return RedirectToAction("Edit", new { Id = roleId });
        }
        // Xóa, gỡ bỏ quyền của User 
        [Route("admin/role/delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(String id)
        {
            var role = await roleManager.FindByIdAsync(id.ToString());

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return Json(new { success = true, message = "Xóa vai trò thành công" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("Index");
            }
        }
        
        [HttpGet]
        [AllowAnonymous]
        [Route("admin/denied")]
        public IActionResult AccessDenied()
        {
            var user = userManager.GetUserAsync(User);
            var info = user;
            return View();
        }

    }
}
