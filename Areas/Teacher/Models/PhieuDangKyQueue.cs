using QLDaoTao.Areas.Admin.Models;

namespace QLDaoTao.Areas.Teacher.Models
{
    public class PhieuDangKyQueue
    {
        private static readonly List<PhieuDangKyNghiDayDayBuVM> _queue = new();

        public void Enqueue(PhieuDangKyNghiDayDayBuVM phieu)
        {
            lock (_queue) // Tránh xung đột
            {
                _queue.Add(phieu);
            }
        }

        public List<PhieuDangKyNghiDayDayBuVM> DequeueAll()
        {
            lock (_queue)
            {
                var items = new List<PhieuDangKyNghiDayDayBuVM>(_queue);
                _queue.Clear();
                return items;
            }
        }
    }
}
