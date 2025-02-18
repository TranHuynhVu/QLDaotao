using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using QLDaoTao.Areas.Admin.Models;
using QLDaoTao.Data;
using QLDaoTao.Data.Enums;
using QLDaoTao.Models;
using QLDaoTao.Services;
using System.Security.Policy;

namespace QLDaoTao.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NewController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public NewController(
            AppDbContext context, IWebHostEnvironment hostEnvironment
            )
        {
            webHostEnvironment = hostEnvironment;
            _context = context;
        }
        // GET: NewController
        [Route("admin/news")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [Route("admin/news/list")]
        public async Task<IActionResult> getList()
        {
            var news = await _context.News.ToListAsync();
            return Json(new
            {
                data = news
            });
        }
        // GET: NewController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NewController/Create
        [Route("admin/new/create")]
        public ActionResult Create()
        {
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            ViewBag.isPost = false;
            // ViewData["categories"] = new MultiSelectList(categories, "Id", "title");
            return View();
        }

        // POST: NewController/Create
        [Route("admin/new/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppNew model, IFormFile files, string postStatus)
        {
            try
            {
                ModelState.Remove("files");
                if (ModelState.IsValid)
                {
                    if (files != null)
                    {
                        // Thêm ảnh vào wwwroot của NewsPost(Image)
                        var fileName = Path.GetFileName(files.FileName);
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        var fileExtension = Path.GetExtension(fileName);
                        var newFileName = String.Concat(myUniqueFileName, fileExtension);

                        var filepath =
                       new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads/Posts")).Root + $@"\{newFileName}";

                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            files.CopyTo(fs);
                            fs.Flush();
                        }

                        var newImageName = newFileName;
                        model.image = "/Uploads/Posts/" + newImageName.ToString();
                    }
                    else
                    {
                        model.image = "/Uploads/Posts/noNewImage.jpg";

                    }
                    model.status = postStatus == "Published" ? 1 : 2;
                    model.created_at = DateTime.Now;
                    string slug = GenerateSlug.Slugify(model.title);
                    model.slug = slug;
                    var data = model;
                    // Kiểm tra slug đã tồn tại không, nếu tồn tại ta gộp slug với số ngày hiện tại
                    var existedSlug = this.CheckSlug(model.slug);
                    if (existedSlug)
                    {
                        model.slug = GenerateSlug.Slugify(model.title) + "-" + DateTime.Now.Day.ToString();
                    }
                    _context.News.Add(model);
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

        // GET: NewController/Edit/5
        [Route("admin/new/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = await _context.News.FindAsync(id);

            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            ViewBag.isPost = true;

            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: NewController/Edit/5
        [Route("admin/new/edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppNew model, IFormFile files, string postStatus)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            ModelState.Remove("files");
            if (ModelState.IsValid)
            {
                try
                {
                    if (files != null)
                    {
                        // Thêm ảnh vào wwwroot của NewsPost(Image)
                        var fileName = Path.GetFileName(files.FileName);
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        var fileExtension = Path.GetExtension(fileName);
                        var newFileName = String.Concat(myUniqueFileName, fileExtension);

                        var filepath =
                       new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads/Posts")).Root + $@"\{newFileName}";

                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            files.CopyTo(fs);
                            fs.Flush();
                        }

                        var newImageName = newFileName;
                        model.image = "/Uploads/Posts/" + newImageName.ToString();
                    }
                    model.status = postStatus == "Published" ? 1 : 2;
                    model.created_at = DateTime.Now;
                    string slug = GenerateSlug.Slugify(model.title);
                    model.slug = slug;
                    var data = model;
                    var existedSlug = this.CheckSlug(model.slug);
                    if (existedSlug)
                    {
                        model.slug = GenerateSlug.Slugify(model.title) + "-" + DateTime.Now.Day.ToString();
                    }
                    model.updated_at = DateTime.Now;
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    TempData["msg"] = "Cập nhật thành công!";
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: NewController/Delete/5
        [HttpDelete]
        [Route("admin/new/delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var post = _context.News.Find(id);
            if (post == null)
            {
                return NotFound();
            }
            _context.News.Remove(post);
            _context.SaveChanges();

            return Json(new { success = true, message = "Xóa danh mục thành công" });
        }

        [HttpPost]
        [Route("admin/new/upload_ckeditor")]

        public IActionResult UploadCKEditor(IFormFile upload)
        {
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
            var paths = Path.Combine(Directory.GetCurrentDirectory(), webHostEnvironment.WebRootPath, "Uploads/Posts", fileName);
            var stream = new FileStream(paths, FileMode.Create);
            upload.CopyToAsync(stream);
            //return new JsonResult(new { path= "/uploads/"+ fileName });
            return new JsonResult(new
            {
                uploaded = 1,
                fileName = upload.FileName,
                url = "/Uploads/Posts/" + fileName
            });
        }

        [HttpGet]
        [Route("admin/new/filebrowse")]
        public IActionResult FileBrowse()
        {
            var dir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(),
              webHostEnvironment.WebRootPath, "Uploads/Posts"));
            ViewBag.fileInfos = dir.GetFiles();
            return View("FileBrowse");
        }
        [Route("checkslug/{slug}")]
        public bool CheckSlug(string slug)
        {
            return _context.News.Count(x => x.slug.ToLower() == slug.ToLower()) > 0;
        }
    }
}
