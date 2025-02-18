using Microsoft.EntityFrameworkCore;
using QLDaoTao.Data;
using QLDaoTao.Models;

namespace QLDaoTao.Areas.Admin.Services
{
    public class ItemLopHocPhanService : ILopHocPhan
    {
        private readonly AppDbContext _context;
        public ItemLopHocPhanService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<LopHocPhan>> ListByTeacher(int maGV)
        {
            var list = await _context.LopHocPhan.Include(x => x.HocPhan)
                                                .Include(x => x.GiangVien)
                                                .Where(x => x.GiangVien.MaGV == maGV)
                                                .ToListAsync();
            return list;
        }
    }
}
