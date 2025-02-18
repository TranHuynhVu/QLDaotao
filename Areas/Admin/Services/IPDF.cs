using QLDaoTao.Areas.Admin.Models;

namespace QLDaoTao.Areas.Admin.Services
{
    public interface IPDF
    {
        Task<byte[]> GeneratePdfFromHtml(string htmlContent);
    }
}
