using QLDaoTao.Models;
using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Areas.Teacher.Models
{
    public class LopHocPhanNghiDayDayBuCTO
    {
        [Required]
        public string IdLopHocPhan { get; set; }
        [Required]
        public int Thu { get; set; }
        [Required]
        public int TuTiet { get; set; }
        [Required]
        public int DenTiet { get; set; }
        [Required]
        public string Phong { get; set; }
        [Required]
        public string LyDo { get; set; }
        [Required]
        public DateTime NgayXinNghi { get; set; }
        [Required]
        public DateTime NgayDayBu { get; set; }
    }
}
