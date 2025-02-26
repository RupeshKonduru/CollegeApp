using CollegeApp.Model;
using Microsoft.AspNetCore.JsonPatch;

namespace CollegeApp.Data.Repository
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id, bool exicutewithtracking);
        Task<Student> GetByNameAsync(string name);
        Task<int> UpdateAsync(Student student);
        Task<bool> DeleteAsyn(Student student);
        Task<int> UpdatePartialAsync(Student student);
        Task<int> CreateAsyn(Student student);

    }
}
