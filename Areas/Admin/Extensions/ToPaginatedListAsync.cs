using Microsoft.EntityFrameworkCore;
using QLDaoTao.Areas.Admin.Models;

namespace QLDaoTao.Areas.Admin.Extensions
{
    public static class ToPaginatedListAsync
    {
        public static async Task<PaginatedList<T>> GetPaginatedList<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}