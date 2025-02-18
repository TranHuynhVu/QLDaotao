using QLDaoTao.Areas.Admin.Services;
using QLDaoTao.Areas.Teacher.Models;
using QLDaoTao.Data;
using Quartz;

namespace QLDaoTao.Areas.Teacher.Jobs
{
    public class ProcessPhieuDangKyJob : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IPhieuDangKyNghiDayDayBu _phieuDangKyNghiDayDayBu;

        public ProcessPhieuDangKyJob(IServiceScopeFactory serviceScopeFactory, IPhieuDangKyNghiDayDayBu phieuDangKyNghiDayDayBu)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _phieuDangKyNghiDayDayBu = phieuDangKyNghiDayDayBu;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var queue = scope.ServiceProvider.GetRequiredService<PhieuDangKyQueue>();
            var phieuDangKyService = scope.ServiceProvider.GetRequiredService<IPhieuDangKyNghiDayDayBu>(); // Lấy service từ scope

            var danhSachPhieu = queue.DequeueAll();
            if (danhSachPhieu.Count > 0)
            {
                foreach (var item in danhSachPhieu)
                {
                    var result = await _phieuDangKyNghiDayDayBu.Create(item);
                    if (!result)
                    {
                        Console.WriteLine("[Quartz] Có lỗi xảy ra khi thêm!");
                    }
                }
                Console.WriteLine($"[Quartz] Đã xử lý {danhSachPhieu.Count} phiếu đăng ký.");
            }
        }
    }
}
