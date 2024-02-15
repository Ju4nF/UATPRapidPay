using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UATP.RapidPay.Data.Repository.IRepository;

namespace UATP.RapidPay.Data.Repository
{
    public class RapidPayRepository<T> : IRapidPayRepository<T> where T : class
    {
        private readonly RapidPayDbContext _db;
        internal DbSet<T> _dbSet;

        public RapidPayRepository(RapidPayDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await SaveAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
