using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Models
{
    public class AppUnit
    {
        [Key] public int Id { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập Mã đơn vị"), DisplayName("Mã đơn vị")]
        public string MaDV { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Tên đơn vị"), DisplayName("Tên đơn vị")]
        public string TenDV { get; set; }
        public string? DiaChi { get; set; }
        public String? Tel { get; set; }
        public String? Fax { get; set; }
        public String? Email { get; set; }
        public String? WebDV { get; set; }
        public int? Status { get; set; } = 1;
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
