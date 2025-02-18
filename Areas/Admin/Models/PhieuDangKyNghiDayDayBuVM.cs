using QLDaoTao.Models;

namespace QLDaoTao.Areas.Admin.Models
{
    public class PhieuDangKyNghiDayDayBuVM
    {
        public int Id { get; set; }
        public int MaGV { get; set; }
        public string TenGV { get; set; }
        public int SoBuoiXinNghi {  get; set; }
        public int TrangThai { get; set; }
        public string TrangThaiStr { get; set; }
        public string SDT { get; set; }
        public string BoMon { get; set; }
        public string Khoa { get; set; }
        public string? LyDo { get; set; }
        public List<BanSaoVBCTDiKem> BanSaoVBCTDiKem { get; set; }
        public List<LopHocPhanNghiDayDayBuVM> LopHocPhanNghiDayDayBuVM { get; set; }
        public String NgayTao {  get; set; }
        public DateTime NgayTaoDT { get; set; }
    }
}
