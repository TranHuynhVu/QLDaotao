using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace QLDaoTao.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string? Receiver { get; set; } = null; // id của người nhận(UserCode || UserName)
        public string Title { get; set; }
        public string Description { get; set; }     
        public DateTime? CreatedAt { get; set; }= DateTime.Now;
        public DateTime? ReadAt { get;set; }
        [Required]
        [DefaultValue(0)]
        public int Status { get; set; }
        public string TypeNoti { get; set; } // Student, Admin, Teacher
    }
}
