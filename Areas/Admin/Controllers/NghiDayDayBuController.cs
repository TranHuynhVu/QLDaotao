using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLDaoTao.Areas.Admin.Models;
using QLDaoTao.Areas.Admin.Services;
using QLDaoTao.Data;
using System.Diagnostics;

namespace QLDaoTao.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class NghiDayDayBuController : Controller
    {
        private readonly IPhieuDangKyNghiDayDayBu _phieuDangKyNghiDayDayBu;
        private readonly AppDbContext _context;

        public NghiDayDayBuController(IPhieuDangKyNghiDayDayBu phieuDangKyNghiDayDayBu, AppDbContext context)
        {
            _phieuDangKyNghiDayDayBu = phieuDangKyNghiDayDayBu;
            _context = context;
        }

        [Route("Admin/NghiDayDayBu")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Admin/NghiDayDayBu/List")]
        public async Task<IActionResult> List(string fromDate, string toDate, string status, string khoa)
        {
            var list = await _phieuDangKyNghiDayDayBu.List(fromDate, toDate, status, khoa);
            return Json(new {Data = list});
        }
        [Route("Admin/NghiDayDayBu/Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [Route("Admin/NghiDayDayBu/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                TempData["msg_danger"] = "Thông tin không hợp lệ!";
                return RedirectToAction("Index");
            }
            var phieuDangKy = await _phieuDangKyNghiDayDayBu.Details(id);
            if (id == null)
            {
                TempData["msg_danger"] = "Không tìm thấy chi tiết phiếu đăng ký!";
                return RedirectToAction("Index");
            }
            return View(phieuDangKy);
        }

        [Route("Admin/NghiDayDayBu/Deny/{id}")]
        [HttpPost]
        public async Task<IActionResult> Deny(int id, [FromBody] DenyVM model)
        {
            if(id == null)
            {
                return Json(new { success = false, message = "Thông tin không hợp lệ" });
            }

            if (model.Reason == null)
            {
                return Json(new { success = false, message = "Lý do trống" });
            }

            if (await _phieuDangKyNghiDayDayBu.Edit(id, -1, model.Reason))
            {
                return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
            }
            else { 
                return Json(new { success = false, message = "Cập nhật trạng thái thất bại" });
            }
           
        }

        [Route("Admin/NghiDayDayBu/Edit/{id}/{status}")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, int status)
        {
            if (id == null || status == null)
            {
                return Json(new { success = false, message = "Thông tin không hợp lệ" });
            }

            if (await _phieuDangKyNghiDayDayBu.Edit(id, status, null))
            {
                return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Cập nhật trạng thái thất bại" });
            }

        }
        [Route("Admin/NghiDayDayBu/ExportPDF/{id}")]
        public async Task<IActionResult> ExportPDF(int id)
        {
            var phieuDangKy = await _phieuDangKyNghiDayDayBu.Details(id);
            var pdfBytes = await _phieuDangKyNghiDayDayBu.ExportPDF(phieuDangKy);
            string fileName = $"{phieuDangKy.TenGV}_PhieuDKNghiDayDayBu.pdf";

            // Trả về file PDF cho tải xuống
            return File(pdfBytes, "application/pdf", fileName);
        }
        [Route("Admin/NghiDayDayBu/ExportPDFs")]
        public async Task<IActionResult> ExportPDFs()
        {
            var listToExprot = (await _phieuDangKyNghiDayDayBu.List(null, null, null, null))
                   .Where(x => x.TrangThai == 0)
                   .ToList();
            if (listToExprot == null || !listToExprot.Any())
            {
                return Json(new { error = true, message = "Không tìm thấy phiếu nào để xuất" });
            }
            byte[] pdfBytes = await _phieuDangKyNghiDayDayBu.ExportPDFs(listToExprot);
            if (pdfBytes == null)
            {
                return Json(new { error = true, message = "Không thể tạo file PDF" });
            }

            foreach (var phieu in listToExprot)
            {
                bool success = await _phieuDangKyNghiDayDayBu.Edit(phieu.Id, 1, null);
                if (!success)
                {
                    Console.WriteLine($"Thất bại khi cập nhật phiếu có Id: {phieu.Id}");
                }
            }
            return File(pdfBytes, "application/pdf");
        }
    }
}
