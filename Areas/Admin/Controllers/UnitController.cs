using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLDaoTao.Data;
using QLDaoTao.Models;

namespace QLDaoTao.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UnitController : Controller
    {
        private readonly AppDbContext _context;
        public UnitController(
            AppDbContext context
            )
        {
            _context = context;
        }
        // GET: UnitController
        [Route("admin/units")]
       
        public async Task<IActionResult> Index()
        {
            var appUnits = await _context.Units.ToListAsync();
            return View();
        }
        [Route("admin/units/list")]
        public async Task<IActionResult> getList()
        {
            var appUnits = await _context.Units.ToListAsync();
            return Json(new
            {
                data = appUnits
            });
        }
        // GET: UnitController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UnitController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnitController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppUnit unit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Units.Add(unit);
                    _context.SaveChanges();
                    TempData["msg"] = "Thêm mới thành công!";
                    return RedirectToAction("Index");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UnitController/Edit/5
        [Route("admin/unit/edit/{id}")]
        public ActionResult Edit(int id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var unit = _context.Units.Find(id);
            if (unit == null)
            {
                return NotFound();
            }
            return View(unit);
        }

        // POST: UnitController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/unit/edit/{id}")]
        public ActionResult Edit(AppUnit unit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Units.Update(unit);
                    TempData["msg"] = "Cập nhật thành công!";
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UnitController/Delete/5
        [HttpDelete]
        [Route("admin/unit/delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var unit = _context.Units.Find(id);
            if (unit == null)
            {
                return NotFound();
            }
            _context.Units.Remove(unit);
            _context.SaveChanges();
            
            return Json(new { success = true, message = "Xóa đơn vị thành công" });
        }

        // POST: UnitController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
