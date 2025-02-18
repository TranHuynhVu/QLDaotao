using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.ViewModels;

public class LoginStudentVM
{
    [Required(ErrorMessage = "Mã sinh viên là bắt buộc.")]
    public string? MaSV { get; set; }

    [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name ="Ghi nhớ tài khoản")]
    public bool RememberMe { get; set; }
}
