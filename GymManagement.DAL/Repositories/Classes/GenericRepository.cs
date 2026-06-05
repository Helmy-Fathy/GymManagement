using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;
        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _dbContext.Set<TEntity>() : _dbContext.Set<TEntity>().AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id, ct);
        }

        public async Task<int> AddAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbContext.Set<TEntity>().Add(entity);
            return await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<int> UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbContext.Set<TEntity>().Update(entity);
            return await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            return await _dbContext.SaveChangesAsync(ct);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return _dbContext.Set<TEntity>().AsNoTracking().AnyAsync(predicate, ct);
        }
    }
}
