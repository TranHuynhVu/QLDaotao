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

namespace QLDaoTao.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class NghiDayDayBuController : Controller
    {
        private readonly IPhieuDangKyNghiDayDayBu _phieuDangKyNghiDayDayBu;
        private readonly UserManager<AppUser> _userManager;
        private readonly PhieuDangKyQueue _queue;
        public NghiDayDayBuController(IPhieuDangKyNghiDayDayBu phieuDangKyNghiDayDayBu, UserManager<AppUser> userManager, PhieuDangKyQueue queue)
        {
            _userManager = userManager;
            _phieuDangKyNghiDayDayBu = phieuDangKyNghiDayDayBu;
            _queue = queue;
        }

        [Route("Teacher/NghiDayDayBu/Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [Route("Teacher/NghiDayDayBu/Create")]
        [HttpPost]
        public async Task<IActionResult> Create(PhieuDangKyNghiDayDayBuCTO model)
        {
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

                _queue.Enqueue(phieuDangKy);

                TempData["success"] = "Phiếu đăng ký đã được nhận và đang chờ xử lý !";
                return View();
            }
            else
            {
                TempData["error"] = "Thông tin không hợp lệ !";
                return View(model);
            }
        }
    }
}
