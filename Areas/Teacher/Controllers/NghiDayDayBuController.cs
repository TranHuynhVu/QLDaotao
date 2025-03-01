using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using QLDaoTao.Areas.Admin.Models;
using QLDaoTao.Areas.Admin.Services;
using QLDaoTao.Areas.Teacher.Models;
using QLDaoTao.Data;
using QLDaoTao.Models;
using QLDaoTao.Services;

namespace QLDaoTao.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class NghiDayDayBuController : Controller
    {
        private readonly IPhieuDangKyNghiDayDayBu _phieuDangKyNghiDayDayBu;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHangFire _hangFire;
        public NghiDayDayBuController(IPhieuDangKyNghiDayDayBu phieuDangKyNghiDayDayBu, UserManager<AppUser> userManager, IHangFire hangFire)
        {
            _userManager = userManager;
            _phieuDangKyNghiDayDayBu = phieuDangKyNghiDayDayBu;
            _hangFire = hangFire;
        }

        [Route("Teacher/NghiDayDayBu/Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [Route("Teacher/NghiDayDayBu/Create")]
        [HttpPost]
        public async Task<IActionResult> Create(PhieuDangKyNghiDayDayBuDTO model)
        {
            if(model == null)
            {
                TempData["error"] = "Thông tin không hợp lệ !";
                return View();
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var phieuDangKy = new PhieuDangKyNghiDayDayBuVM();
                phieuDangKy.SoBuoiXinNghi = model.SoBuoiXinNghi;
                phieuDangKy.NgayTaoDT = DateTime.Now;
                phieuDangKy.TrangThai = 0;
                phieuDangKy.MaGV = int.Parse(user.UserName);
                phieuDangKy.BanSaoVBCTDiKem = new List<BanSaoVBCTDiKem>();
                phieuDangKy.LopHocPhanNghiDayDayBuVM = new List<LopHocPhanNghiDayDayBuVM>();

                if (model.BanSaoVBCTDiKem != null)
                {
                    foreach (var item in model.BanSaoVBCTDiKem)
                    {
                        var pre = DateTime.Now.Ticks.ToString();
                        var fileName = Path.GetFileName(item.FileName);
                        var filePath = Path.Combine("wwwroot/Uploads/NghiDayDayBu", pre + fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await item.CopyToAsync(stream);
                        }

                        var vbct = new BanSaoVBCTDiKem
                        {
                            DuongDan = "/Uploads/NghiDayDayBu/" + pre + fileName,
                            // MoTa IdPhieuDangKyDayBu là các giá trị tạm thời
                            MoTa = "",
                            IdPhieuDangKyDayBu = 0
                        };
                        phieuDangKy.BanSaoVBCTDiKem.Add(vbct);
                    }
                }

                if (model.LopHocPhanNghiDayDayBu == null)
                {
                    TempData["error"] = "Phải chọn các LHP để đăng ký nghỉ dạy - dạy bù !";
                    return View(model);
                }

                foreach (var item in model.LopHocPhanNghiDayDayBu)
                {
                    var lhp = new LopHocPhanNghiDayDayBuVM();
                    lhp.Id = item.IdLopHocPhan;
                    lhp.NgayXinNghiDT = item.NgayXinNghi;
                    lhp.NgayDayBuDT = item.NgayDayBu;
                    lhp.ThuDayBu = item.Thu;
                    lhp.TuTietDayBu = item.TuTiet;
                    lhp.DenTietDayBu = item.DenTiet;
                    lhp.Phong = item.Phong;
                    lhp.LyDo = item.LyDo;

                    phieuDangKy.LopHocPhanNghiDayDayBuVM.Add(lhp);
                }

                _hangFire.ScheduleJob(() => _phieuDangKyNghiDayDayBu.Create(phieuDangKy), TimeSpan.FromSeconds(3));
              
                TempData["success"] = "Phiếu đăng ký đã được nhận và đang chờ xử lý !";
                
                return View();
            }
            else
            {
                TempData["error"] = "Thông tin không hợp lệ !";
                return View(model);
            }
        }
        [Route("Teacher/NghiDayDayBu/History/")]
        [HttpGet]
        public async Task<IActionResult> History(int page = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            var list = await _phieuDangKyNghiDayDayBu.ListByTeacher(int.Parse(user.UserName));
            int pageSize = 5; // Số lượng card trên mỗi trang
            var totalItems = list.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            var PageList = list.Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            return View(PageList);
        }

        [Route("Teacher/NghiDayDayBu/Details/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if(id == null)
            {
                TempData["error"] = "Thông tin không hợp lệ !";
                return View();
            }
            var phieu = await _phieuDangKyNghiDayDayBu.Details(id);
            if (phieu == null)
            {
                TempData["error"] = "Không tìm thấy phiếu đăng ký !";
                return View();
            }
            return View(phieu);
        }

        [Route("Teacher/NghiDayDayBu/ExportPDF/{id}")]
        public async Task<IActionResult> ExportPDF(int id)
        {
            if (id == null)
            {
                TempData["error"] = "Thông tin không hợp lệ !";
                return View();
            }
            var phieuDangKy = await _phieuDangKyNghiDayDayBu.Details(id);
            if (phieuDangKy == null)
            {
                TempData["error"] = "Không tìm thấy phiếu đăng ký !";
                return View();
            }
            var pdfBytes = await _phieuDangKyNghiDayDayBu.ExportPDF(phieuDangKy);
            if (pdfBytes == null)
            {
                TempData["error"] = "Không thể chuyển đổi dữ liệu !";
                return View();
            }
            string fileName = $"{phieuDangKy.TenGV}_PhieuDKNghiDayDayBu.pdf";

            // Trả về file PDF cho tải xuống
            return File(pdfBytes, "application/pdf", fileName);
        }
        [Route("Teacher/NghiDayDayBu/Edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit (int id)
        {
            if(id == null)
            {
                TempData["error"] = "Thông tin không hợp lệ !";
                return View();
            }
            var phieu = await _phieuDangKyNghiDayDayBu.Details(id);
            if(phieu == null)
            {
                TempData["error"] = "Không tìm thấy phiếu đăng ký !";
                return View();
            }
            return View(phieu);
        }

        [Route("Teacher/NghiDayDayBu/Edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit (int id, PhieuDangKyNghiDayDayBuVM model, List<IFormFile> vbct)
        {
            if( id == null || model == null)
            {
                TempData["error"] = "Thông tin không hợp lệ !";
                return View();
            }
            if (ModelState.IsValid)
            {
                var result = await _phieuDangKyNghiDayDayBu.EditForTeacher(model);
                if (!result)
                {
                    TempData["error"] = "Cập nhật thất bại !";
                    return View(model);
                }
                TempData["success"] = "Phiếu đăng ký đã được cập nhật và đang chờ xử lý !";
                return RedirectToAction("Details", new { id = id });

            }
            TempData["error"] = "Thông tin không hợp lệ !";
            return View(model);
        }
        [Route("Teacher/NghiDayDayBu/Cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            if(id == null)
            {
                TempData["error"] = "Thông tin không hợp lệ !";
                return RedirectToAction("Details", new { id = id });
            }
            var result = await _phieuDangKyNghiDayDayBu.Edit(id,-1,"Giảng viên rút phiếu");
            if (!result)
            {
                TempData["error"] = "Rút phiếu thất bại !";
                return RedirectToAction("Details", new { id = id });
            }
            TempData["success"] = "Rút phiếu thành công!";
            return RedirectToAction("Details", new { id = id });
        }
    }
}