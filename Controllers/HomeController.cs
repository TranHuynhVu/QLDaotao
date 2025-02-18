using QLDaoTao.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace QLDaoTao.Controllers;

//[Authorize]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    [Route("/giang-vien")]
    public IActionResult GiangVien()
    {   
        return View();
    }
    [Route("/sinh-vien")]
    public IActionResult SinhVien()
    {
        return View();
    }
    [Route("/phong-ban")]
    public IActionResult PhongBan()
    {
        return View();
    }
    [Route("/khoa")]
    public IActionResult Khoa()
    {
        return View();
    }
    [Route("/doan-the")]
    public IActionResult DoanThe()
    {
        return View();
    }
    [Route("/tin-tuc-su-kien")]
    public IActionResult TinTucSuKien()
    {
        return View();
    }
    [Route("/thong-bao")]
    public IActionResult ThongBao()
    {
        return View();
    }
    [Route("~/bai-viet/{slug}")]
    public IActionResult ChiTiet(string slug)
    {
        return View();
    }
    [Route("/chi-tiet-bai-viet")]
    public IActionResult Detail()
    {
        return View();
    }
    [Route("/van-ban-bieu-mau")]
    public IActionResult VanBanBieuMau()
    {
        return View();
    }
    [Route("/ke-hoach")]
    public IActionResult KeHoach()
    {
        return View();
    }
    [Route("/lien-he")]
    public IActionResult LienHe()
    {
        return View();
    }
    //Đào tạo Đại học và Đào tạo Thạc Sỹ
    [Route("/nganh-dao-tao/dao-tao-dai-hoc")]
    public IActionResult DaoTaoDaiHoc(string type)
    {
        return View();
    }
    [Route("/nganh-dao-tao/dao-tao-thac-sy")]
    public IActionResult DaoTaoThacSy(string type)
    {
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}