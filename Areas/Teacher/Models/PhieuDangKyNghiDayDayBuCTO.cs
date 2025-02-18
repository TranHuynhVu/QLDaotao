using QLDaoTao.Models;
using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Areas.Teacher.Models
{
    public class PhieuDangKyNghiDayDayBuCTO
    {
        [Required]
        public int SoBuoiXinNghi { get; set; }
        [Required]
        public List<LopHocPhanNghiDayDayBuCTO> LopHocPhanNghiDayDayBu { get; set; }
        public List<IFormFile> BanSaoVBCTDiKem { get; set; } 
    }
}
