using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Models
{
    public class PhieuDangKyDayBu
    {
        [Key]   
        public int Id { get; set; }
        [Required]
        public int SoBuoiXinNghi { get; set; }
        public string? LyDo { get; set; }
        [Required]
        public int TrangThai { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public ICollection<LopHocPhanPhieuDangKyDayBu> LopHocPhanPhieuDangKyDayBu { get; set; }
    }
}
