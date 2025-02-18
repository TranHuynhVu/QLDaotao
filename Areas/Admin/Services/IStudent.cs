using QLDaoTao.Models;

namespace QLDaoTao.Areas.Admin.Services
{
    public interface IStudent
    {
        IEnumerable<AppStudent> getStudentAll();
        int totalStudent();
        int numberPage(int totalProduct, int limit);
        IEnumerable<AppStudent> paginationStudent(int start, int limit, String txtSearch);

    }
}
