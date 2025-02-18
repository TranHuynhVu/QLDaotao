using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Areas.Admin.Models
{
    public class CreateRoleVM
    {
     
        [Required(ErrorMessage = "Vui lòng nhập tên Vai trò"), DisplayName("Tên vai trò")]
        [Display(Name = "Role")]
        public string Name { get; set; }
    }
}
