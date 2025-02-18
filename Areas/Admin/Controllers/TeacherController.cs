using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLDaoTao.Areas.Admin.Data;
using QLDaoTao.Data;
using QLDaoTao.Models;
using QLDaoTao.Areas.Admin.Models;
using System.Reflection;
using System.Security;
using QLDaoTao.ViewModels;

namespace QLDaoTao.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TeacherController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, AppDbContext _context) : Controller
    {
       
        [Route("admin/teachers")]
        // GET: TeacherController
        public ActionResult Index()
        {
            return View();
        }
        [Route("admin/teachers/list")]
        public async Task<IActionResult> getList()
        {
            var teachers = await _context.Teachers.ToListAsync();
            return Json(new
            {
                data = teachers
            });
        }
        // GET: TeacherController/Details/5
        [Route("admin/teacher/detail/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(String id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }
        [Route("admin/teacher/reset-password/{id}")]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.UserName == teacher.MaGV);

            if (user == null)
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            ViewBag.FullName = user.Name;
            string token = TokenGenerator.GenerateToken();
            var model = new ResetPasswordModel { Token = token, Id = id };
            return View(model);
        }
        [Route("admin/teacher/reset-password/{id}")]
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
        public string CalculateNewUnitCode(string unitCode)
        {           
            var numberOfUsers = _context.Teachers.Where(u => u.MaDV == unitCode).Count();
            var newUnitCode = unitCode + numberOfUsers.ToString("D4");
            var check = _context.Teachers.Where(u => u.MaDV == unitCode).FirstAsync();
            if (check == null)
            {
                return newUnitCode;
            }else
            {
                return newUnitCode+1;
            }
         

        }
        // GET: TeacherController/Create
        [Route("admin/teacher/create")]
        [HttpGet]
        public IActionResult Create()
        {
            List<AppUnit> units = new List<AppUnit>();
            units = _context.Units.ToList();
            ViewBag.Unit = units;
            return View();
        }

        // POST: TeacherController/Create
        [HttpPost]
        public async Task<IActionResult> Create(TeacherVM model)
        {
            if (ModelState.IsValid)
            {
                var selectedUnitCode = model.MaDV;
                var MaGiangVien = CalculateNewUnitCode(selectedUnitCode);
                AppUser user = new()
                {
                    Name = model.LastName + " " + model.FirstName,
                    UserName = MaGiangVien,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = MaGiangVien,
                    NormalizedEmail = MaGiangVien,
                    Status = 1,
                    Role = "Teacher"
                };

                var result = await userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    //await signInManager.SignInAsync(user, false);
                    var userId = user.Id;
                    var dataTeacher = new AppTeacher
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        HoTenGV = model.LastName + " " + model.FirstName,
                        MaGV = MaGiangVien,
                        Password = model.Password,
                        PasswordHash = user.PasswordHash,
                        UserName = MaGiangVien,
                        Email = model.Email,
                        MaDV = model.MaDV,
                        Mobile = model.Mobile,
                        GioiTinh = model.GioiTinh,
                        DOB = model.DOB,
                        Status = "H",
                        DiaChi = model.DiaChi,
                        Chucvu = model.Chucvu,
                        Chucdanh = model.Chucdanh,
                        Hocvi = model.Hocvi,
                        Tel = model.Tel,
                        UserId = userId,
                        LockoutEnabled = true,
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                };
                    dataTeacher.CreationTime = DateTime.Now;
                    _context.Teachers.Add(dataTeacher);
                    _context.SaveChanges();
                    TempData["msg"] = "Thêm mới thành công!";
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        // GET: TeacherController/Edit/5
        [Route("admin/teacher/edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(String id)
        {
            List<AppUnit> units = new List<AppUnit>();
            units = _context.Units.ToList();
            ViewBag.Unit = units;
            if (id == null)
            {
                return NotFound();
            }
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: TeacherController/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/teacher/edit/{id}")]
        public async Task<IActionResult> Edit(EditTeacherVM model)
        {
            try
            {
                var teacher = await _context.Teachers.FindAsync(model.Id.ToString());
                if (teacher == null)
                {
                    ViewBag.ErrorMessage = $"Teacher with Id = {model.Id} cannot be found";
                    return View("NotFound");
                }
                if (ModelState.IsValid)
                {
                    teacher.FirstName = model.FirstName;
                    teacher.LastName = model.LastName;
                    teacher.HoTenGV = model.LastName + " " + model.FirstName;
                    teacher.MaDV = model.MaDV;
                    teacher.Email = model.Email;
                    teacher.Mobile = model.Mobile;
                    teacher.GioiTinh = model.GioiTinh;
                    teacher.DOB = model.DOB;
                    teacher.Status = teacher.Status;//model.Status != null ? 1 : 0;
                    teacher.DiaChi = model.DiaChi;
                    teacher.Chucvu = model.Chucvu;
                    teacher.Chucdanh = model.Chucdanh;
                    teacher.Hocvi = model.Hocvi;
                    teacher.Tel = model.Tel;
                    teacher.LastModificationTime = DateTime.Now;
                    _context.Teachers.Update(teacher);                    
                    _context.SaveChanges();
                    TempData["msg"] = "Cập nhật thành công!";
                    return RedirectToAction("Index");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        [HttpDelete]
        [Route("/admin/teacher/delete/{id}")]
        public async Task<ActionResult> Delete(String id)
        {
            var teacher = _context.Teachers.Find(id.ToString());
            if (teacher == null)
            {
                return NotFound();
            }
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.UserName == teacher.MaGV);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            _context.Teachers.Remove(teacher);
            _context.SaveChanges();

            return Json(new { success = true, message = "Xóa thành công!" });
        }

        // POST: TeacherController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(String id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
