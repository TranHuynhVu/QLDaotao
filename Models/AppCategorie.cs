using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Models
{
    public class AppCategorie
    {
        [Key] public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề"), DisplayName("Tiêu đề")]
        public string? title { get; set; }
        public string? slug { get; set; }
        public string? summary { get; set; }
        public int? parent_id { get; set; }
        public int? order { get; set; }
        public int? status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

    }
}
