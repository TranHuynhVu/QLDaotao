using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDaoTao.Models
{
    public class GiangVien
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int MaGV { get; set; }
        [Required]
        public string HoTen { get; set; }
        [Required]
        public string SDT { get; set; }
        [ForeignKey(nameof(BoMon))]
        public int IdBoMon { get; set; }
        public BoMon BoMon { get; set; }

    }
}
