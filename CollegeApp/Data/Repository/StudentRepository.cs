using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CollegeDbContext _dbcontext;
        private readonly IMapper _mapper;
        public StudentRepository(CollegeDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }
        public async Task<int> CreateAsyn(Student student)
        {
            _dbcontext.Students.Add(student);
            await _dbcontext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> DeleteAsyn(Student student)
        {
            _dbcontext.Remove(student);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _dbcontext.Students.ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id, bool exicutewithtracking)
        {
            if (exicutewithtracking)
                return await _dbcontext.Students.AsNoTracking().Where(a => a.Id == id).FirstOrDefaultAsync();
            return await _dbcontext.Students.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Student> GetByNameAsync(string name)
        {
            return await _dbcontext.Students.Where(a => a.StdName.ToLower().Equals(name.ToLower())).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAsync(Student student)
        {
            _dbcontext.Update(student);
            await _dbcontext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<int> UpdatePartialAsync(Student student)
        {
            _dbcontext.Update(student);
            await _dbcontext.SaveChangesAsync();
            return student.Id;
        }
    }
}
