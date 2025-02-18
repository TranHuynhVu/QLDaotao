using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using QLDaoTao.Data;
using QLDaoTao.Models;

namespace QLDaoTao.Areas.Admin.Services
{
    public class ItemStudentService : IStudent
    {
        private readonly AppDbContext _db;
        private List<AppStudent> students = new List<AppStudent>();
        public ItemStudentService(AppDbContext db)
        {
            _db = db;
            this.students = _db.Students.ToList();
        }
        public IEnumerable<AppStudent> getStudentAll()
        {
            return students;
        }
        public int totalStudent()
        {
            return students.Count;
        }
        public int numberPage(int totalProduct, int limit)
        {
            float numberpage = totalProduct / limit;
            return (int)Math.Ceiling(numberpage);
        }
        public IEnumerable<AppStudent> paginationStudent(int start, int limit, string txtSearch)
        {
            var data = (from s in _db.Students select s);
            if (!String.IsNullOrEmpty(txtSearch))
            {
                var search = txtSearch.Trim();
                //data = data.Where(s => s.TenViet.Contains(search));
                data = data.Where(s =>
                 string.IsNullOrEmpty(search) ||
                 s.TenViet.Contains(search) ||
                 s.Email.Contains(search) ||
                 s.MaSV.Contains(search)
             );


            }
            var dataStudent = data.OrderByDescending(x => x.Id).Skip(start).Take(limit);
            return dataStudent.ToList();
        }
    }

}
