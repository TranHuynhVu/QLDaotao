using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Models
{
    public class AppNew
    {
        [Key] public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề"), DisplayName("Tiêu đề")]
        public string? title { get; set; }
        public string? slug { get; set; }
        public string? image { get; set; }
        public string? description { get; set; }
        public string? keyword { get; set; }
        public int? views { get; set; }
        public int? feature { get; set; }
        public int? category_id { get; set; }
        public int? user_id { get; set; }
        public string? content { get; set; }
        public string? Author { get; set; }
        public int? status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }
}
