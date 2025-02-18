using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLDaoTao.Data;
using QLDaoTao.Models;
using QLDaoTao.Services;
using System.Reflection;

namespace QLDaoTao.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        [Route("admin/categories")]
        [HttpGet]
        // GET: CategoryController
       
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }
        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        [Route("admin/categories/create")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [Route("admin/categories/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppCategorie categorie)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string slug = GenerateSlug.Slugify(categorie.title);
                    categorie.slug = slug;
                    categorie.created_at = DateTime.Now;
                    _context.Categories.Add(categorie);
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

        // GET: CategoryController/Edit/5
        [Route("admin/categories/edit/{id}")]
        public ActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categorie = _context.Categories.Find(id);
            if (categorie == null)
            {
                return NotFound();
            }
            return View(categorie);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/categories/edit/{id}")]
        public ActionResult Edit(AppCategorie categorie)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string slug = GenerateSlug.Slugify(categorie.title);
                    categorie.slug = slug;
                    categorie.updated_at = DateTime.Now;
                    _context.Categories.Update(categorie);
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
        [Route("admin/categories/delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categorie = _context.Categories.Find(id);
            if (categorie == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(categorie);
            _context.SaveChanges();

            return Json(new { success = true, message = "Xóa danh mục thành công" });
        }

      
    }
}
