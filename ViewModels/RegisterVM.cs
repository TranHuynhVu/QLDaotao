using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.ViewModels;

public class RegisterVM
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    public string? UserCode { get; set; }

    [Required (ErrorMessage ="FirstName is required")]
    public string? FirstName { get; set; }

    [Required (ErrorMessage ="LastName is required")]
    public string? LastName { get; set; }

    [Compare("Password", ErrorMessage = "Passwords don't match.")]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }

    [DataType(DataType.MultilineText)]
    public string? Address { get; set; }
}
