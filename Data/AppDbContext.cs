using QLDaoTao.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace QLDaoTao.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<AppStudent> Students { get; set; }
    public DbSet<AppUnit> Units { get; set; }
    public DbSet<AppCategorie> Categories { get; set; }
    public DbSet<AppNew> News { get; set; }
    public DbSet<AppTeacher> Teachers { get; set; }
    public DbSet<AppBanner> Banners { get; set; }
    public DbSet<AppLink> Links { get; set; }
    public DbSet<AppBackup> Backups { get; set; }
    public DbSet<Khoa> Khoa { get; set; }
    public DbSet<BoMon> BoMon { get; set; }
    public DbSet<HocPhan> HocPhan { get; set; }
    public DbSet<GiangVien> GiangVien { get; set; }
    public DbSet<LopHocPhan> LopHocPhan { get; set; }
    public DbSet<PhieuDangKyDayBu> PhieuDangKyDayBu { get; set; }
    public DbSet<BanSaoVBCTDiKem> BanSaoVBCTDiKem { get; set; }
    public DbSet<LopHocPhanPhieuDangKyDayBu> LopHocPhanPhieuDangKyDayBu { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Thiết lập khóa chính cho bảng trung gian
        modelBuilder.Entity<LopHocPhanPhieuDangKyDayBu>()
            .HasKey(x => new { x.IdLopHocPhan, x.IdPhieuDangKyDayBu });

        // Thiết lập quan hệ giữa Enrollment và Student
        modelBuilder.Entity<LopHocPhanPhieuDangKyDayBu>()
            .HasOne(x=> x.LopHocPhan)
            .WithMany(x => x.LopHocPhanPhieuDangKyDayBu)
            .HasForeignKey(x => x.IdLopHocPhan);

        // Thiết lập quan hệ giữa Enrollment và Course
        modelBuilder.Entity<LopHocPhanPhieuDangKyDayBu>()
            .HasOne(x => x.PhieuDangKyDayBu)
            .WithMany(x => x.LopHocPhanPhieuDangKyDayBu)
            .HasForeignKey(x => x.IdPhieuDangKyDayBu);
    }
}
