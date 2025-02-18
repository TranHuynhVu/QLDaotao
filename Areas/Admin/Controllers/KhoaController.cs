using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLDaoTao.Areas.Admin.Services;

namespace QLDaoTao.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class KhoaController : Controller
    {
        private readonly IKhoa _khoa;

        public KhoaController(IKhoa khoa)
        {
            _khoa = khoa;
        }
        [Route("Admin/Khoa/List")]
        public async Task<IActionResult> List()
        {
            var list = await _khoa.List();
            return Json(new {Data = list});
        }
    }
}
