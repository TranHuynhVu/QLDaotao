using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Models
{
    public class HocPhan
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Ten { get; set; }
    }
}
