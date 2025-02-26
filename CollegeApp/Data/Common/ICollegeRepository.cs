using System.Linq.Expressions;

namespace CollegeApp.Data.Common
{
    public interface ICollegeRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Expression<Func<T, bool>> filter, bool exicutewithtracking);
        Task<T> GetByNameAsync(Expression<Func<T, bool>> filter);
        Task<T> UpdateAsync(T T);
        Task<bool> DeleteAsyn(T T);
        Task<T> UpdatePartialAsync(T T);
        Task<T> CreateAsyn(T T);

    }
}
