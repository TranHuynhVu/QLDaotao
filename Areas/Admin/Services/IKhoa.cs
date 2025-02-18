using QLDaoTao.Models;

namespace QLDaoTao.Areas.Admin.Services
{
    public interface IKhoa
    {
        public Task<List<Khoa>> List();
    }
}
