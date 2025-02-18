using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLDaoTao.Data;
using QLDaoTao.Models;

namespace QLDaoTao.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LinkController : Controller
    {
        private readonly AppDbContext _context;
        public LinkController(AppDbContext context)
        {
            _context = context;
        }
        // GET: LinkController
        [Route("admin/links")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var links = await _context.Links.ToListAsync();
            return View(links);
        }

        // GET: LinkController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LinkController/Create
        [Route("admin/link/create")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: LinkController/Create
        [Route("admin/link/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppLink model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Links.Add(model);
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

        // GET: LinkController/Edit/5
        [Route("admin/link/edit/{id}")]
        public ActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var link = _context.Links.Find(id);
            if (link == null)
            {
                return NotFound();
            }
            return View(link);
        }

        // POST: LinkController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/link/edit/{id}")]
        public ActionResult Edit(AppLink model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Links.Update(model);
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

        [HttpDelete]
        [Route("admin/link/delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var link = _context.Links.Find(id);
            if (link == null)
            {
                return NotFound();
            }
            _context.Links.Remove(link);
            _context.SaveChanges();

            return Json(new { success = true, message = "Xóa danh mục thành công" });
        }

       
    }
}
