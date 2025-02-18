using QLDaoTao.Areas.Admin.Models;

namespace QLDaoTao.Areas.Admin.Services
{
    public interface IPhieuDangKyNghiDayDayBu
    {
        public Task<List<PhieuDangKyNghiDayDayBuVM>> List(string fromDate, string toDate, string status, string khoa);
        public Task<List<PhieuDangKyNghiDayDayBuVM>> ListByTeacher(int maGV);
        public Task<PhieuDangKyNghiDayDayBuVM> Details(int id);
        public Task<bool> Create(PhieuDangKyNghiDayDayBuVM model);
        public Task<bool> Edit(int id, int status, string? reason);
        public Task<byte[]> ExportPDF(PhieuDangKyNghiDayDayBuVM phieuDangKy);
        public Task<byte[]> ExportPDFs(List<PhieuDangKyNghiDayDayBuVM> phieuDangKy);
    }
}
