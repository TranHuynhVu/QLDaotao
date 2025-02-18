using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Areas.Admin.Models
{
    public class CreateNewVM
    {
     
        [Required(ErrorMessage = "Vui lòng nhập tên Vai trò"), DisplayName("Tên vai trò")]
        [Display(Name = "title")]
        public string Title { get; set; }
    }
}
