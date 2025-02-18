using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace QLDaoTao.Models
{
    public class AppBackup
    {
        [Key] public int Id { get; set; }
        public string Path { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
