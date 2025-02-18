using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDaoTao.Models
{
    public class LopHocPhan
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public int TuTiet { get; set; }
        [Required]
        public int DenTiet { get; set; }
        [Required]
        public int Thu { get; set; }
        [Required]
        public string Phong { get; set; }
        [ForeignKey(nameof(GiangVien))]
        public int IdGiangVien { get; set; }
        [ForeignKey(nameof(HocPhan))]
        public string IdHocPhan { get; set; }
        public GiangVien GiangVien { get; set; }
        public HocPhan HocPhan { get; set; }
        public ICollection<LopHocPhanPhieuDangKyDayBu> LopHocPhanPhieuDangKyDayBu { get; set; }
    }
}
