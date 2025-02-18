using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Areas.Admin.Models
{
    public class ResetPasswordModel
    {
     
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới"), DisplayName("Mật khẩu")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập Xác nhật mật khẩu"), DisplayName("Xác nhận Mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
        public string Id { get; set; }
        public string Token { get; set; }
    }
}
