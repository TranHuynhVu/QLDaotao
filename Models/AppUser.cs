using QLDaoTao.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace QLDaoTao.Models;

public class AppUser : IdentityUser
{
    [StringLength(100)]
    [MaxLength(100)]
    [Required]
    [Display(Name = "Họ và tên")]
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? UrlAvatar { get; set; }
   
    [Required(ErrorMessage = "Vui lòng nhập Họ"), DisplayName("Họ")]
    public string? LastName { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập Tên"), DisplayName("Tên")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập Email"), DisplayName("Email")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    public string Email { get; set; }
    public string? NormalizedEmail { get; set; }
    public string? Password { get; set; }
    public string? Phone { get; set; }
    public string? Tel { get; set; }
    public string? Role { get; set; }
    public int? Status { get; set; }
    public int? Gender { get; set; }
    public DateOnly ? DateOfBirth { get; set; }
}
