using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Areas.Admin.Models
{
    public class StudentVM
    {
        public Guid Id { get; set; }
        public string? MaSV { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Họ"), DisplayName("Họ")]
        public string? Ho { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Tên"), DisplayName("Tên")]
        public string? Ten { get; set; }
        public string? TenViet { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Ngày sinh"), DisplayName("Ngày sinh")]
        public DateOnly? DOB { get; set; }
        public bool? GioiTinh { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Địa chie"), DisplayName("Địa chỉ")]
        public string? DiaChi { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại"), DisplayName("Số điện thoại")]
        public string? Tel { get; set; }
        public string? Mobile { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email"), DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [DataType(DataType.EmailAddress)]
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
}
