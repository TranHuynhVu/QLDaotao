using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLDaoTao.Data;
using QLDaoTao.Models;

namespace QLDaoTao.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SystemController(AppDbContext _context) : Controller
    {
        [Route("admin/backup-database")]
        [HttpGet]
        public IActionResult BackupDatabase()
        {
            try
            {
                TakeBackup();
                return Content("Backup thành công!");
            }
            catch (Exception ex)
            {
                return Content("Backup thất bại! Lỗi: " + ex.Message);
            }
        }

        private void TakeBackup()
        {
            var Server = "DESKTOP-52R69HK\\MSSQLSERVER01";
            var Database = "QLDaoTao";
            var Username = "admin";
            var Password = "Admin@789";
            var connString = "Data Source=" + Server + ";Initial Catalog=" + Database + ";User Id=" + Username + ";Password=" + Password + ";Encrypt=False;";

            try
            {
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    var currentTime = DateTime.Now;
                    var txtName = "QLDT_" + currentTime.ToString("yyyyMMddHHmmss");
                    SqlCommand command = conn.CreateCommand();
                    //command.CommandText = "BACKUP DATABASE " + Database + " TO DISK = 'C:\\backupdb\\" + txtName + ".bak'";
                    var backupFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Backups");
                    if (!Directory.Exists(backupFolderPath))
                    {
                        Directory.CreateDirectory(backupFolderPath);
                    }

                    command.CommandText = "BACKUP DATABASE " + Database + " TO DISK = '" + Path.Combine(backupFolderPath, txtName + ".bak") + "'";

                    command.ExecuteNonQuery();
                    var newBackup = new AppBackup
                    {
                        Path = txtName,
                        CreatedAt = DateTime.Now
                    };

                    _context.Backups.Add(newBackup);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Backup thất bại! Lỗi: " + ex.Message);
            }
        }
        [Route("admin/backups")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var backups = await _context.Backups.ToListAsync();
            return View(backups);
        }

        [HttpDelete]
        [Route("/admin/backup/delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var backup = _context.Backups.Find(id);
            if (backup == null)
            {
                return NotFound();
            }
            _context.Backups.Remove(backup);
            _context.SaveChanges();

            return Json(new { success = true, message = "Xóa backup thành công" });
        }

    }
}
