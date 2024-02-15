using System.Linq.Expressions;

namespace UATP.RapidPay.Data.Repository.IRepository
{
    public interface IRapidPayRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task SaveAsync();
    }
}
