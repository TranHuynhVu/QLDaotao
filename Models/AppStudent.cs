using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Models;

public class AppStudent : IdentityUser
{
    [StringLength(100)]
    [MaxLength(100)]
    [Required]
    public string? MaSV { get; set; }
    public string? Ho { get; set; }
    public string? Ten { get; set; }
    public string? TenViet { get; set; }
    public DateOnly? DOB { get; set; }
    public bool? GioiTinh { get; set; }
    public string? DiaChi { get; set; }
    public string? Tel { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? EmailDCT { get; set; }
    public string? DiemTS { get; set; }
    public string? AccNo { get; set; }
    public string? Password { get; set; }
    public string? Status { get; set; }
    public string? GhiChu { get; set; }
    public string? MaNH { get; set; }
    public string? SCMND { get; set; }
    public string? TenKD { get; set; }
    public string? Specia { get; set; }
    public string? DiemRL { get; set; }
    public string? CDRNN { get; set; }
    public string? CDRTH { get; set; }
    public string? SCMND_IMG { get; set; }
    public string? CapDT { get; set; }
    public string? KS { get; set; }
    public string? Dantoc { get; set; }
    public string? NoiSinh { get; set; }
    public string? QDTT { get; set; }
    public string? Role { get; set; }

    public int? isVisible { get; set; }
}