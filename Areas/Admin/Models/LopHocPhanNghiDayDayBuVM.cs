namespace QLDaoTao.Areas.Admin.Models
{
    public class LopHocPhanNghiDayDayBuVM
    {
        public string Id { get; set; }
        public string TenHocPhan { get; set; }
        public int ThuTKB { get; set; }
        public int TuTietTKB { get; set; }
        public int DenTietTKB { get; set;}
        public int ThuDayBu { get; set; }
        public int TuTietDayBu { get; set; }
        public int DenTietDayBu{ get; set; }
        public String NgayXinNghi { get; set; }
        public String NgayDayBu { get; set; }
        public DateTime NgayXinNghiDT { get; set; }
        public DateTime NgayDayBuDT { get; set; }
        public string Phong { get; set; }
        public string LyDo { get; set; }

    }
}
