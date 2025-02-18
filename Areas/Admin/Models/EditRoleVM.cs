using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QLDaoTao.Areas.Admin.Models
{
    public class EditRoleVM
    {

        public EditRoleVM()
        {
            Users = new List<string>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "Tên vai trò là bắt buộc")]
        public string Name { get; set; }

        public List<string> Users { get; set; }
    }
}
