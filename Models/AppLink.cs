using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace QLDaoTao.Models
{
    public class AppLink
    {
        [Key] public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề"), DisplayName("Tiêu đề")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập URL"), DisplayName("URL")]
        public string? Link { get; set; }
        public int? Status { get; set; }
        public int? Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
