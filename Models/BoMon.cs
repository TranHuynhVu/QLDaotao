using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDaoTao.Models
{
    public class BoMon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Ten { get; set; }
        [ForeignKey(nameof(Khoa))]
        public int IdKhoa { get; set; }
        public Khoa Khoa { get; set; }
    }
}
