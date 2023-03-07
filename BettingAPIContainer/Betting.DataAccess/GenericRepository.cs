using Betting.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Betting.DataAccess
{
    public class GenericRepository<TEntity, TContext> : IDbRepository<TEntity>
       where TEntity : BaseEntity
       where TContext : BettingApiDbContext
    {
        protected readonly TContext _context;
        public GenericRepository(TContext context)
        {
            _context = context;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            return (await _context.Set<TEntity>().AddAsync(entity)).Entity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() => _context.Set<TEntity>().Remove(entity));
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await Task.Run(() => _context.Set<TEntity>().Update(entity));
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task AddRangeAsync(System.Collections.Generic.List<TEntity> entities)
        {
            await _context.AddRangeAsync(entities);
        }

        public async Task<TEntity> FindByExpressionAsync(Expression<Func<TEntity, bool>> func, string[] included)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            if (included != null)
            {
                foreach (var item in included)
                {
                    query = query.Include(item);
                }
            }

            return await query.FirstOrDefaultAsync(func);
        }

        public async Task<System.Collections.Generic.List<TEntity>> FindAllByExpressionAsync(Expression<Func<TEntity, bool>> func)
        {
            return await _context.Set<TEntity>()                                    
                                 .Where(func)
                                 .ToListAsync();
        }
    }
}
