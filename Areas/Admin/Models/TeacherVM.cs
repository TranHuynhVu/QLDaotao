using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QLDaoTao.Areas.Admin.Models
{ 
    public class TeacherVM
    {
        public Guid Id { get; set; }
        public string? HoTenGV { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Họ"), DisplayName("Họ")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Tên"), DisplayName("Tên")]
        public string FirstName { get; set; }
        public string? MaGV { get; set; }
        public DateOnly? DOB { get; set; }
        public bool? GioiTinh { get; set; }
        public string? DiaChi { get; set; }
        public string? Tel { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? EmailDCT { get; set; }
        public string? MaDV { get; set; }
        public string? Hocvi { get; set; }
        public string? Chucdanh { get; set; }
        public string? Chucvu { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public string? WebSite { get; set; }
        public string? TenViet { get; set; }
        public string? Status { get; set; }
        public string? AccRight { get; set; }
        public DateOnly? PublishDate { get; set; }
        public string? ExtraProperties { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public DateTime? CreationTime { get; set; }
        public string? CreatorId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string? UserId { get; set; }
    }

}
