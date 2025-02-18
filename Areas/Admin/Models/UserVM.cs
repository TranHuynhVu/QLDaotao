using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Areas.Admin.Models;

public class UserVM
{
  
   
    [Required(ErrorMessage = "Vui lòng nhập Họ"), DisplayName("Họ")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập Tên"), DisplayName("Tên")]
    public string FirstName { get; set; }
    public string? Phone { get; set; }
   
    [Required(ErrorMessage = "Vui lòng nhập Email"), DisplayName("Email")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [DataType(DataType.MultilineText)]
    public string? Address { get; set; }
    public string? Role { get; set; }
    public int? Gender { get; set; }
    public string? Tel { get; set; }
    public string? UrlAvatar { get; set; }
    public int? Status { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu người dùng"), DisplayName("Mật khẩu")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    public IFormFile? IUrlAvatar { get; set; }




}
