using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Models
{
    public class Khoa
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Ten { get; set; }
    }
}
