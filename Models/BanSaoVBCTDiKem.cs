using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDaoTao.Models
{
    public class BanSaoVBCTDiKem
    {
        [Key]
        public int Id { get; set; }
        public string MoTa { get; set; }
        [Required]
        public string DuongDan { get; set; }
        [ForeignKey(nameof(PhieuDangKyDayBu))]
        public int IdPhieuDangKyDayBu { get; set; }
        public PhieuDangKyDayBu PhieuDangKyDayBu { get; set;}
    }
}
