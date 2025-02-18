using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace QLDaoTao.Areas.Admin.Models
{
    public class DenyVM
    {
        [Required(ErrorMessage = "Vui lòng nhập lý do"), DisplayName("Nhập lý do")]
        [Display(Name = "Reason")]
        public string Reason { get; set; }
    }
}
