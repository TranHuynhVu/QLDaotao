using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace QLDaoTao.Models
{
    public class AppBanner
    {
        [Key] public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề"), DisplayName("Tiêu đề")]
        public string Title { get; set; }
        public string? Link { get; set; }
        public string? Image { get; set; }
        public int? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
