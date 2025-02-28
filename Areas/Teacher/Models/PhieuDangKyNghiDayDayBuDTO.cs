using QLDaoTao.Models;
using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Areas.Teacher.Models
{
    public class PhieuDangKyNghiDayDayBuDTO
    {
        [Required]
        public int SoBuoiXinNghi { get; set; }
        [Required]
        public List<LopHocPhanNghiDayDayBuDTO> LopHocPhanNghiDayDayBu { get; set; }
        public List<IFormFile>? BanSaoVBCTDiKem { get; set; } 
    }
}
