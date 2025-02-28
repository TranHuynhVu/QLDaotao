namespace QLDaoTao.Areas.Admin.Models
{
    public class CheckPhieuVM
    {
        public int? Id { get; set; }
        public int? MaGv { get; set; }
        public DateTime? CreateAt { get; set; }
        public List<LhpPdkVm>? lhpPdkVms { get; set; }
    }
}
