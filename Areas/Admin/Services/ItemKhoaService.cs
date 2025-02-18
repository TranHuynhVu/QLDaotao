using Microsoft.EntityFrameworkCore;
using QLDaoTao.Data;
using QLDaoTao.Models;

namespace QLDaoTao.Areas.Admin.Services
{
    public class ItemKhoaService : IKhoa
    {
        private readonly AppDbContext _context;

        public ItemKhoaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Khoa>> List()
        {
            var list = await _context.Khoa.ToListAsync();
            return list;
        }
    }
}
