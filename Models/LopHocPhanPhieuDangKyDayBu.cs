namespace QLDaoTao.Models
{
    public class LopHocPhanPhieuDangKyDayBu
    {
        public int IdPhieuDangKyDayBu { get; set; }
        public PhieuDangKyDayBu PhieuDangKyDayBu {  get; set; }
        public string IdLopHocPhan {  get; set; }
        public LopHocPhan LopHocPhan {  get; set; }
        public int Thu {  get; set; }
        public int TuTiet { get; set; }
        public int DenTiet { get; set; }
        public string Phong {  get; set; }
        public string LyDo { get; set; }
        public DateTime NgayXinNghi { get; set; }
        public DateTime NgayDayBu { get; set; }
    }
}
