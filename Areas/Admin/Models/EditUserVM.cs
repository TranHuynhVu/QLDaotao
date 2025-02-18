using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QLDaoTao.Areas.Admin.Models
{ 
    public class EditUserVM
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? Phone { get; set; }
        [Required]
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
        public IFormFile? IUrlAvatar { get; set; }
    }

}
