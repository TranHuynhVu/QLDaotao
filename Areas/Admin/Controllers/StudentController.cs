using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLDaoTao.Areas.Admin.Data;
using QLDaoTao.Data;
using QLDaoTao.Models;
using QLDaoTao.Areas.Admin.Models;
using NuGet.DependencyResolver;
using QLDaoTao.Data.Enums;
using QLDaoTao.Services;
using QLDaoTao.Areas.Admin.Services;

namespace QLDaoTao.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StudentController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, AppDbContext _context, IStudent _student) : Controller
    {
        // GET: StudentController
        [Route("Admin/Student")]
        [Route("admin/students")]
        public ActionResult Index(string txtSearch,int? page = 0)
        {
            int limit = 10;
            int start;
            if (page > 0)
            {
                page = page;
            }
            else
            {
                page = 1;
            }
            start = (int)(page - 1) * limit;

            ViewBag.pageCurrent = page;

            int totalStudent = _student.totalStudent();

            ViewBag.totalStudent = totalStudent;

            ViewBag.numberPage = _student.numberPage(totalStudent, limit);
            
            var data = _student.paginationStudent(start, limit, txtSearch);
            ViewBag.txtSearch = "";
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                int totalStudent_ = data.Count();
                ViewBag.totalStudent = totalStudent_;
                ViewBag.numberPage = _student.numberPage(totalStudent_, limit);
            }

            return View(data);
        }
        [Route("admin/students/list")]
        public async Task<IActionResult> getList()
        {
            var teachers = await _context.Students.ToListAsync();
            return Json(new
            {
                data = teachers
            });
        }
        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StudentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: StudentController/Edit/5
        [Route("admin/student/edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(String id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/student/edit/{id}")]
        public async Task<IActionResult> Edit(StudentVM model)
        {
            try
            {
                var student = await _context.Students.FindAsync(model.Id.ToString());
                if (student == null)
                {
                    ViewBag.ErrorMessage = $"student with Id = {model.Id} cannot be found";
                    return View("NotFound");
                }
                if (ModelState.IsValid)
                {
                    student.Ho = model.Ho;
                    student.Ten = model.Ten;
                    student.TenViet = model.Ho + " " + model.Ten;
                    student.Email = model.Email;
                    student.Mobile = model.Mobile;
                    student.GioiTinh = model.GioiTinh;
                    student.DOB = model.DOB;
                    student.DiaChi = model.DiaChi;
                    student.Tel = model.Tel;
                    _context.Students.Update(student);
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
        [Route("/admin/student/delete/{id}")]
        public async Task<ActionResult> Delete(String id)
        {
            var student = _context.Students.Find(id.ToString());
            if (student == null)
            {
                return NotFound();
            }
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.UserName == student.MaSV);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            _context.Students.Remove(student);
            _context.SaveChanges();

            return Json(new { success = true, message = "Xóa thành công!" });
        }
        [Route("admin/student/reset-password/{id}")]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.UserName == student.MaSV);

            if (user == null)
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            ViewBag.FullName = user.Name;
            string token = TokenGenerator.GenerateToken();
            var model = new ResetPasswordModel { Token = token, Id = id };
            return View(model);
        }
        [Route("admin/student/reset-password/{id}")]
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

        [HttpPost]
        [Route("admin/student/update-status/{id}/{status}")]
        public async Task<ActionResult> UpdateStatus(string id, int status)
        {
            var student = _context.Students.Find(id.ToString());
            if (student == null)
            {
                return NotFound();
            }
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.UserName == student.MaSV);
            if (user != null)
            {
                user.Status = status;
                _context.Students.Update(student);
                _context.SaveChanges();
            }
            student.isVisible = status;
            _context.Students.Update(student);
            _context.SaveChanges();
            return Json(new { success = true, message = "Thay đổi trạng thái thành công!" });

        }
    }
}
