using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using QLDaoTao.Data;
using QLDaoTao.Models;
using System.Reflection;

namespace QLDaoTao.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BannerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnviroment;
        public BannerController(AppDbContext context, IWebHostEnvironment hostingEnviroment)
        {
            _context = context;
            _hostingEnviroment = hostingEnviroment;
        }
        [Route("admin/banners")]
        [HttpGet]
        // GET: BannerController
        public async Task<IActionResult> Index()
        {
            var banners = await _context.Banners.ToListAsync();
            return View(banners);
        }

        // GET: BannerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BannerController/Create
        [Route("admin/banners/create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: BannerController/Create
        [Route("admin/banners/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppBanner banner, IFormFile files)
        {
            if (files != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var fileName = Path.GetFileName(files.FileName);
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        var fileExtension = Path.GetExtension(fileName);
                        var newFileName = String.Concat(myUniqueFileName, fileExtension);

                        var filepath =
                       new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads/Banners")).Root + $@"\{newFileName}";

                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            files.CopyTo(fs);
                            fs.Flush();
                        }

                        var newImageName = newFileName;
                        banner.Image = "/Uploads/Banners/" + newImageName.ToString();
                        banner.CreatedAt = DateTime.Now;
                        _context.Banners.Add(banner);
                        _context.SaveChanges();
                        TempData["msg"] = "Thêm mới thành công!";
                        return RedirectToAction("Index");
                    }
                    return View();
                }
                catch
                {
                    return View();
                }

            }
            TempData["msg_danger"] = "Vui lòng chọn thêm ảnh";
            return RedirectToAction(nameof(Create));
        }

        // GET: BannerController/Edit/5
        [Route("admin/banner/edit/{id}")]
       
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var banner = await _context.Banners.FindAsync(id);
                      

            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }
        // POST: BannerController/Edit/5
        [Route("admin/banner/edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppBanner banner, IFormFile files)
        {
            if (id != banner.Id)
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
                        var fileName = Path.GetFileName(files.FileName);
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        var fileExtension = Path.GetExtension(fileName);
                        var newFileName = String.Concat(myUniqueFileName, fileExtension);

                        var filepath =
                       new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads/Banners")).Root + $@"\{newFileName}";

                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            files.CopyTo(fs);
                            fs.Flush();
                        }

                        var newImageName = newFileName;
                        banner.Image = "/Uploads/Banners/" + newImageName.ToString();
                    }
                    banner.UpdatedAt = DateTime.Now;
                    _context.Update(banner);
                    _context.Attach(banner).State = EntityState.Modified;
                    _context.Entry(banner).Property(x => x.Image).IsModified = files != null;
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

        [HttpDelete]
        [Route("/admin/banner/delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var banner = _context.Banners.Find(id);
            if (banner == null)
            {
                return NotFound();
            }
            _context.Banners.Remove(banner);
            _context.SaveChanges();

            return Json(new { success = true, message = "Xóa banner thành công" });
        }

        // POST: BannerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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

        public string UploadedFile(IFormFile IUrlAvatar)
        {
            string uniqueFileName = null;
            if (IUrlAvatar != null)
            {
                int len = IUrlAvatar.FileName.Length - 4;// Trừ cho 4 vì kiểm tra đuôi file
                string fileTag = IUrlAvatar.FileName.Substring(len);
                string[] acceptedExtensions = { ".jpg", ".jpeg", ".png", ".PNG", ".JPG", ".JPEG" };

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
    }
}
