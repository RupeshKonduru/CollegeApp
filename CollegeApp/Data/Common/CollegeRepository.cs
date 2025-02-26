using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CollegeApp.Data.Common
{
    public class CollegeRepository<T> : ICollegeRepository<T> where T : class
    {
        private readonly CollegeDbContext _dbcontext;
        private DbSet<T> _dbSet;
        public CollegeRepository(CollegeDbContext dbcontext)
        {
            _dbcontext = dbcontext;
            _dbSet = _dbcontext.Set<T>();
        }
        public async Task<T> CreateAsyn(T dbRecord)
        {
            _dbSet.Add(dbRecord);
            await _dbcontext.SaveChangesAsync();
            return dbRecord;
        }

        public async Task<bool> DeleteAsyn(T dbRecord)
        {
            _dbSet.Remove(dbRecord);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> filter, bool exicutewithtracking)
        {
            if (exicutewithtracking)
                return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetByNameAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<T> UpdateAsync(T dbRecord)
        {
            _dbSet.Update(dbRecord);
            await _dbcontext.SaveChangesAsync();
            return dbRecord;
        }

        public async Task<T> UpdatePartialAsync(T dbRecord)
        {
            _dbcontext.Update(dbRecord);
            await _dbcontext.SaveChangesAsync();
            return dbRecord;
        }
    }
}
