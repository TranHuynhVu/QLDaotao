using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using QLDaoTao.Areas.Admin.Models;
using QLDaoTao.Data;
using QLDaoTao.Models;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace QLDaoTao.Areas.Admin.Services
{
    public class ItemPhieuDangKyNghiDayDayBuService : IPhieuDangKyNghiDayDayBu
    {
        private readonly AppDbContext _context;
        private readonly IPDF _pdf;

        public ItemPhieuDangKyNghiDayDayBuService(AppDbContext context, IPDF pdf)
        {
            _context = context;
            _pdf = pdf;
        }

        public async Task<List<PhieuDangKyNghiDayDayBuVM>> List(string fromDate, string toDate, string status, string khoa)
        {
            DateTime? fromDateTime = string.IsNullOrEmpty(fromDate) ? null : DateTime.Parse(fromDate);
            DateTime? toDateTime = string.IsNullOrEmpty(toDate) ? null : DateTime.Parse(toDate);

            // Lấy danh sách phiếu đăng ký nghỉ dạy dạy bù
            var listPhieuDayBu = await _context.PhieuDangKyDayBu
                                                .Where(x => (fromDateTime == null || x.CreatedAt >= fromDateTime) &&
                                                    (toDateTime == null || x.CreatedAt <= toDateTime) &&
                                                    (string.IsNullOrEmpty(status) || x.TrangThai.ToString() == status))
                                                .Select(x => new PhieuDangKyNghiDayDayBuVM
                                                {
                                                    Id = x.Id,
                                                    SoBuoiXinNghi = x.SoBuoiXinNghi,
                                                    TrangThai = x.TrangThai,
                                                    TrangThaiStr = x.TrangThai == 0 ? "Chờ xử lý" :
                                                                   x.TrangThai == 1 ? "Đang xử lý" :
                                                                   x.TrangThai == 2 ? "Đã xử lý" :
                                                                   x.TrangThai == 3 ? "Đã nhận" :
                                                                   x.TrangThai == 4 ? "Hết hạn" :
                                                                   "Bị từ chối",
                                                    MaGV = x.CreatedBy,
                                                    NgayTao = x.CreatedAt.ToString("dd-MM-yyyy"),
                                                    LyDo = x.LyDo
                                                })
                                                .ToListAsync();
            // Thêm danh sách lớp học phần đăng ký nghỉ dạy dạy bù cho phiếu đăng ký
            foreach(var x in listPhieuDayBu)
            {
                var listLHP = await _context.LopHocPhanPhieuDangKyDayBu.Include(x => x.LopHocPhan)
                                                                       .ThenInclude(x => x.HocPhan)
                                                                       .Where(p => p.IdPhieuDangKyDayBu == x.Id)
                                                                       .Select(x => new LopHocPhanNghiDayDayBuVM
                                                                       {
                                                                           Id = x.IdLopHocPhan,
                                                                           TenHocPhan = x.LopHocPhan.HocPhan.Ten,
                                                                           ThuTKB = x.LopHocPhan.Thu,
                                                                           TuTietTKB = x.LopHocPhan.TuTiet,
                                                                           DenTietTKB = x.LopHocPhan.DenTiet,
                                                                           ThuDayBu = x.Thu,
                                                                           TuTietDayBu = x.TuTiet,
                                                                           DenTietDayBu = x.DenTiet,
                                                                           Phong = x.Phong,
                                                                           NgayXinNghi = x.NgayXinNghi.ToString("dd-MM-yyyy"),
                                                                           NgayDayBu = x.NgayDayBu.ToString("dd-MM-yyyy"),
                                                                           LyDo = x.LyDo
                                                                       })
                                                                       .ToListAsync();
                x.LopHocPhanNghiDayDayBuVM = listLHP;
            }
            // Thêm danh sách bản sao văn bản chứng từ đi kèm cho phiếu đăng ký
            foreach(var x in listPhieuDayBu )
            {
                var listBanSaoVBCT = await _context.BanSaoVBCTDiKem.Where(b => b.IdPhieuDangKyDayBu == x.Id)
                                                                   .ToListAsync();
                x.BanSaoVBCTDiKem = listBanSaoVBCT;
            }
            // Thêm thông tin giảng viên cho phiếu đăng ký
            foreach(var x in listPhieuDayBu)
            {
                var gv = await _context.GiangVien.Include(x => x.BoMon)
                                                 .ThenInclude(x => x.Khoa)
                                                 .Where(gv => gv.MaGV == x.MaGV)
                                                 .FirstOrDefaultAsync();
                x.TenGV = gv.HoTen;
                x.SDT = gv.SDT;
                x.BoMon = gv.BoMon.Ten;
                x.Khoa = gv.BoMon.Khoa.Ten;

            }

            listPhieuDayBu = listPhieuDayBu.Where(x => string.IsNullOrEmpty(khoa) || x.Khoa.ToString() == khoa).ToList();

            return listPhieuDayBu;
        }
        
        public async Task<PhieuDangKyNghiDayDayBuVM> Details(int id)
        {
            // Lấy ra phiếu đăng ký cần tìm
            var phieuDangKy = await _context.PhieuDangKyDayBu
                                                .Where(x => x.Id == id)
                                                .Select(x => new PhieuDangKyNghiDayDayBuVM
                                                {
                                                    Id = x.Id,
                                                    SoBuoiXinNghi = x.SoBuoiXinNghi,
                                                    TrangThai = x.TrangThai,
                                                    TrangThaiStr = x.TrangThai == 0 ? "Chờ xử lý" :
                                                                   x.TrangThai == 1 ? "Đang xử lý" :
                                                                   x.TrangThai == 2 ? "Đã xử lý" :
                                                                   x.TrangThai == 3 ? "Đã nhận" :
                                                                   x.TrangThai == 4 ? "Hết hạn" :
                                                                   "Bị từ chối",
                                                    MaGV = x.CreatedBy,
                                                    NgayTao = x.CreatedAt.ToString("dd-MM-yyyy"),
                                                    LyDo = x.LyDo
                                                })
                                                .FirstOrDefaultAsync();
            if (phieuDangKy == null)
            {
                return null;
            }
            // Thêm danh sách lớp học phần đăng ký nghỉ dạy dạy bù cho phiếu đăng ký
            var listLHP = await _context.LopHocPhanPhieuDangKyDayBu.Include(x => x.LopHocPhan)
                                                                       .ThenInclude(x => x.HocPhan)
                                                                       .Where(p => p.IdPhieuDangKyDayBu == phieuDangKy.Id)
                                                                       .Select(x => new LopHocPhanNghiDayDayBuVM
                                                                       {
                                                                           Id = x.IdLopHocPhan,
                                                                           TenHocPhan = x.LopHocPhan.HocPhan.Ten,
                                                                           ThuTKB = x.LopHocPhan.Thu,
                                                                           TuTietTKB = x.LopHocPhan.TuTiet,
                                                                           DenTietTKB = x.LopHocPhan.DenTiet,
                                                                           ThuDayBu = x.Thu,
                                                                           TuTietDayBu = x.TuTiet,
                                                                           DenTietDayBu = x.DenTiet,
                                                                           Phong = x.Phong,
                                                                           NgayXinNghi = x.NgayXinNghi.ToString("dd-MM-yyyy"),
                                                                           NgayDayBu = x.NgayDayBu.ToString("dd-MM-yyyy"),
                                                                           LyDo = x.LyDo
                                                                       })
                                                                       .ToListAsync();
            phieuDangKy.LopHocPhanNghiDayDayBuVM = listLHP;
            // Thêm danh sách văn bản chứng từ đi kèm cho phiếu đăng ký
            var listBanSaoVBCT = await _context.BanSaoVBCTDiKem.Where(b => b.IdPhieuDangKyDayBu == phieuDangKy.Id)
                                                                   .ToListAsync();
            phieuDangKy.BanSaoVBCTDiKem = listBanSaoVBCT;
            // Thêm thông tin giảng viên cho phiếu đăng ký
            var gv = await _context.GiangVien.Include(x => x.BoMon)
                                                 .ThenInclude(x => x.Khoa)
                                                 .Where(gv => gv.MaGV == phieuDangKy.MaGV)
                                                 .FirstOrDefaultAsync();
            phieuDangKy.TenGV = gv.HoTen;
            phieuDangKy.SDT = gv.SDT;
            phieuDangKy.BoMon = gv.BoMon.Ten;
            phieuDangKy.Khoa = gv.BoMon.Khoa.Ten;

            return phieuDangKy;
        }

        public async Task<bool> Create(PhieuDangKyNghiDayDayBuVM model)
        {
            if (model == null)
            {
                return false;
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var phieuDangKy = new PhieuDangKyDayBu
                    {
                        SoBuoiXinNghi = model.SoBuoiXinNghi,
                        TrangThai = model.TrangThai,
                        CreatedBy = model.MaGV,
                        CreatedAt = model.NgayTaoDT
                    };

                    _context.PhieuDangKyDayBu.Add(phieuDangKy);
                    var result = await _context.SaveChangesAsync() > 0;

                    if (!result)
                    {
                        Console.WriteLine("Thêm phiếu đăng ký nghỉ dạy dạy bù thất bại!");
                        await transaction.RollbackAsync();
                        return false;
                    }

                    var newestItem = await _context.PhieuDangKyDayBu.OrderByDescending(x => x.Id).FirstOrDefaultAsync();

                    foreach (var vbct in model.BanSaoVBCTDiKem)
                    {
                        var item = new BanSaoVBCTDiKem
                        {
                            MoTa = vbct.MoTa,
                            DuongDan = vbct.DuongDan,
                            IdPhieuDangKyDayBu = newestItem.Id
                        };

                        _context.BanSaoVBCTDiKem.Add(item);
                    }

                    result = await _context.SaveChangesAsync() > 0;
                    if (!result)
                    {
                        Console.WriteLine("Thêm bản sao văn bản chứng từ thất bại!");
                        await transaction.RollbackAsync();
                        return false;
                    }

                    foreach (var lhp in model.LopHocPhanNghiDayDayBuVM)
                    {
                        var item = new LopHocPhanPhieuDangKyDayBu
                        {
                            IdLopHocPhan = lhp.Id,
                            NgayXinNghi = lhp.NgayXinNghiDT,
                            NgayDayBu = lhp.NgayDayBuDT,
                            Thu = lhp.ThuDayBu,
                            TuTiet = lhp.TuTietDayBu,
                            DenTiet = lhp.DenTietDayBu,
                            Phong = lhp.Phong,
                            LyDo = lhp.LyDo,
                            IdPhieuDangKyDayBu = newestItem.Id
                        };

                        _context.LopHocPhanPhieuDangKyDayBu.Add(item);
                    }

                    result = await _context.SaveChangesAsync() > 0;
                    if (!result)
                    {
                        Console.WriteLine("Thêm lớp học phần đăng ký dạy bù thất bại!");
                        await transaction.RollbackAsync();
                        return false;
                    }

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine("Lỗi: " + ex.Message);
                    return false;
                }
            }
        }


        public async Task<bool> Edit (int id, int status, string? reason)
        {
            if (id == null || status == null)
            {
                return false;    
            }
            try
            {
                var item = await _context.PhieuDangKyDayBu.FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return false;
                }
                item.TrangThai = status;
                if(status == -1)
                {
                    if(reason != null)
                    {
                        item.LyDo = reason;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    item.LyDo = null;
                }

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                {
                    return false;
                }

                return true;

                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                return false;
            }
        }

        public async Task<List<PhieuDangKyNghiDayDayBuVM>> ListByTeacher(int maGV)
        {
            var listPhieuDayBu = await _context.PhieuDangKyDayBu
                                                .Where(x => x.CreatedBy == maGV)
                                                .Select(x => new PhieuDangKyNghiDayDayBuVM
                                                {
                                                    Id = x.Id,
                                                    SoBuoiXinNghi = x.SoBuoiXinNghi,
                                                    TrangThai = x.TrangThai,
                                                    TrangThaiStr = x.TrangThai == 0 ? "Chờ xử lý" :
                                                                   x.TrangThai == 1 ? "Đang xử lý" :
                                                                   x.TrangThai == 2 ? "Đã xử lý" :
                                                                   x.TrangThai == 3 ? "Đã nhận" :
                                                                   x.TrangThai == 4 ? "Hết hạn" :
                                                                   "Bị từ chối",
                                                    MaGV = x.CreatedBy,
                                                    NgayTao = x.CreatedAt.ToString("dd-MM-yyyy"),
                                                    LyDo = x.LyDo
                                                })
                                                .ToListAsync();
            // Thêm danh sách lớp học phần đăng ký nghỉ dạy dạy bù cho phiếu đăng ký
            foreach (var x in listPhieuDayBu)
            {
                var listLHP = await _context.LopHocPhanPhieuDangKyDayBu.Include(x => x.LopHocPhan)
                                                                       .ThenInclude(x => x.HocPhan)
                                                                       .Where(p => p.IdPhieuDangKyDayBu == x.Id)
                                                                       .Select(x => new LopHocPhanNghiDayDayBuVM
                                                                       {
                                                                           Id = x.IdLopHocPhan,
                                                                           TenHocPhan = x.LopHocPhan.HocPhan.Ten,
                                                                           ThuTKB = x.LopHocPhan.Thu,
                                                                           TuTietTKB = x.LopHocPhan.TuTiet,
                                                                           DenTietTKB = x.LopHocPhan.DenTiet,
                                                                           ThuDayBu = x.Thu,
                                                                           TuTietDayBu = x.TuTiet,
                                                                           DenTietDayBu = x.DenTiet,
                                                                           Phong = x.Phong,
                                                                           NgayXinNghi = x.NgayXinNghi.ToString("dd-MM-yyyy"),
                                                                           NgayDayBu = x.NgayDayBu.ToString("dd-MM-yyyy"),
                                                                           LyDo = x.LyDo
                                                                       })
                                                                       .ToListAsync();
                x.LopHocPhanNghiDayDayBuVM = listLHP;
            }
            // Thêm danh sách bản sao văn bản chứng từ đi kèm cho phiếu đăng ký
            foreach (var x in listPhieuDayBu)
            {
                var listBanSaoVBCT = await _context.BanSaoVBCTDiKem.Where(b => b.IdPhieuDangKyDayBu == x.Id)
                                                                   .ToListAsync();
                x.BanSaoVBCTDiKem = listBanSaoVBCT;
            }
            // Thêm thông tin giảng viên cho phiếu đăng ký
            foreach (var x in listPhieuDayBu)
            {
                var gv = await _context.GiangVien.Include(x => x.BoMon)
                                                 .ThenInclude(x => x.Khoa)
                                                 .Where(gv => gv.MaGV == x.MaGV)
                                                 .FirstOrDefaultAsync();
                x.TenGV = gv.HoTen;
                x.SDT = gv.SDT;
                x.BoMon = gv.BoMon.Ten;
                x.Khoa = gv.BoMon.Khoa.Ten;

            }

            return listPhieuDayBu;
        }

        public async Task<byte[]> ExportPDF(PhieuDangKyNghiDayDayBuVM phieuDangKy)
        {
            string html = $@"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Document</title>
                </head>
                <body>
                    <div style=""width: 100%; padding: 5rem; box-sizing: border-box; font-family: Arial, Helvetica, sans-serif;"">
                        <table style=""width: 100%;"">
                            <tr>
                                <td style=""width: 50%;"">ĐẠI HỌC ĐÀ NẴNG <br><strong>TRƯỜNG ĐẠI HỌC SƯ PHẠM KỸ THUẬT</strong></td>
                                <td style=""text-align: right; width: 50%;"">Biểu mẫu</td>
                            </tr>
                        </table>
                        <h1 style=""font-family: Times New Roman, Times, serif; text-align: center; margin-top: 2rem;"">PHIẾU ĐĂNG KÝ NGHỈ DẠY - DẠY BÙ</h1>
                        <table style=""width: 100%; border: 1px solid black; border-collapse: collapse;"">
                            <tr style=""border: 1px solid black; border-collapse: collapse;"">
                                <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;""><strong>Họ và tên:</strong></td>
                                <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"">{phieuDangKy.TenGV}</td>
                                <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;""><strong>Số điện thoại:</strong> {phieuDangKy.SDT}</td>
                            </tr>
                            <tr style=""border: 1px solid black; border-collapse: collapse;"">
                                <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;""><strong>Bộ môn:</strong></td>
                                <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"">{phieuDangKy.BoMon} tin</td>
                                <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;""><strong>Khoa:</strong> {phieuDangKy.Khoa}</td>
                            </tr>
                        </table>
                        <h4>Chi tiết xin nghỉ dạy - Kế hoạch dạy bù</h4>
                        <table style=""width: 100%; border: 1px solid black; border-collapse: collapse;"">
                            <tr style=""border: 1px solid black; border-collapse: collapse;"">
                              <th style=""border: 1px solid black; border-collapse: collapse; padding: 10px; width: 10%;"">Tên LHP</th>
                              <th style=""border: 1px solid black; border-collapse: collapse; padding: 10px; width: 15%;"">Tên học phần</th>
                              <th style=""border: 1px solid black; border-collapse: collapse; padding: 10px; width: 12.5%;"">TKB <br> Thứ(Từ - đến)</th>
                              <th style=""border: 1px solid black; border-collapse: collapse; padding: 10px; width: 12.5%;"">Ngày xin nghỉ</th>
                              <th style=""border: 1px solid black; border-collapse: collapse; padding: 10px; width: 12.5%;"">Ngày dạy bù</th>
                              <th style=""border: 1px solid black; border-collapse: collapse; padding: 10px; width: 12.5%;"">Lịch dạy bù <br> Thứ(Từ - đến)</th>
                              <th style=""border: 1px solid black; border-collapse: collapse; padding: 10px; width: 10%;"">Phòng</th>
                              <th style=""border: 1px solid black; border-collapse: collapse; padding: 10px; width: 15%;"">Lý do xin nghỉ</th>
                            </tr> 
            ";

            foreach(var lhp in phieuDangKy.LopHocPhanNghiDayDayBuVM)
            {
                html += $@"
                    <tr style=""border: 1px solid black; border-collapse: collapse;"">
                              <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"">{lhp.Id}</td>
                              <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"">{lhp.TenHocPhan}</td>
                              <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"">{lhp.ThuTKB} ({lhp.TuTietTKB}-{lhp.DenTietTKB})</td>
                              <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"">{lhp.NgayXinNghi}</td>
                              <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"">{lhp.NgayDayBu}</td>
                              <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"">{lhp.ThuDayBu} ({lhp.TuTietDayBu}-{lhp.DenTietDayBu})</td>
                              <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"">{lhp.Phong}</td>
                              <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"">{lhp.LyDo}</td>
                    </tr>
                ";
            };

            html += $@"
             <tr style=""border: 1px solid black; border-collapse: collapse;"">
                                <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"" colspan=""3""><strong>Tổng số buổi xin nghỉ:</strong></td>
                                <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"" colspan=""5"">{phieuDangKy.SoBuoiXinNghi}</td>
                            </tr>
                            <tr style=""border: 1px solid black; border-collapse: collapse;"">
                                <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"" colspan=""3""><strong>Bảng sao văn bản - chứng từ đi kèm:</strong></td>
                                <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"" colspan=""5""></td>
                            </tr>
                        </table>
       
                        <table style=""width: 100%; border: 1px solid black; border-collapse: collapse; margin-top: 2rem;"">
                            <tr style=""border: 1px solid black; border-collapse: collapse;"">
                                <th style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"">Dành cho Thanh tra đào tạo</th>
                                <th style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"">Phê chuẩn của Ban chủ nhiệm Khoa</th>
                                <th style=""border: 1px solid black; border-collapse: collapse; padding: 10px;"">Giảng viên ký ghi rõ họ tên, ngày tháng năm</th>
                            </tr>
                            <tr style=""border: 1px solid black; border-collapse: collapse; height: 6rem;"">
                                <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;""></td>
                                <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;""></td>
                                <td style=""border: 1px solid black; border-collapse: collapse; padding: 10px;""></td>
                            </tr>
                        </table>
                        <p><strong>Lưu ý:</strong>Phiếu đăng ký nghỉ dạy - dạy bù phải được lập thành 2 bản, một bản lưu tại văn phòng Khoa, 1 bản nộp Phòng Đào Tạo sau khi đã có phê duyệt
                        của BCN Khoa. Trong trường hợp đột xuất, giảng viên xin nghỉ phải thông báo qua điện thoại đến Phòng Đào Tạo và hoàn tất phiếu đăng ký này trong vòng 2 ngày kể từ buổi nghỉ. 
                        Giảng viên xin nghỉ dạy bố trí lịch dạy bù và phòng dạy bù trên cơ sở thỏa thuận với sinh viên lớp học phần và thông tin sử dụng phòng đã đăng tải trên Website.</p>
                    </div>
                </body>
                </html>
            ";
            return await _pdf.GeneratePdfFromHtml(html);
        }

        public async Task<byte[]> ExportPDFs(List<PhieuDangKyNghiDayDayBuVM> phieuDangKy)
        {
            using (var memoryStream = new MemoryStream())
            {
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Phiếu đăng ký nghỉ dạy - dạy bù";
                foreach (var phieu in phieuDangKy)
                {
                    byte[] phieuPdfBytes = await ExportPDF(phieu);
                    if (phieuPdfBytes != null && phieuPdfBytes.Length > 0)
                    {
                        using (var phieuMemoryStream = new MemoryStream(phieuPdfBytes))
                        {
                            PdfDocument phieuDocument = PdfReader.Open(phieuMemoryStream, PdfDocumentOpenMode.Import);
                            foreach (var phieuPage in phieuDocument.Pages)
                            {
                                document.AddPage(phieuPage);
                            }
                        }
                    }
                }
                document.Save(memoryStream, false);
                return memoryStream.ToArray();
            }
        }
    }
}
