using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.ViewModels;

public class LoginVM
{
    [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name ="Ghi nhớ tài khoản")]
    public bool RememberMe { get; set; }
}
