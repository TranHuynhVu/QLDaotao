using QLDaoTao.Models;

namespace QLDaoTao.Areas.Admin.Services
{
    public interface ILopHocPhan
    {
        public Task<List<LopHocPhan>> ListByTeacher(int maGV);
    }
}
