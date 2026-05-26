using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using GymManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext dbContext;
        public PlanRepository()
        {
            dbContext = new GymDbContext();
        }
        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<Plan> query = tracking ? dbContext.Plans : dbContext.Plans.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await dbContext.Plans.FindAsync(id, ct);
        }

        public async Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        {
            dbContext.Plans.Add(plan);
            return await dbContext.SaveChangesAsync(ct);
        }

        public async Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            dbContext.Update(plan); 
            return await dbContext.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        {
            dbContext.Plans.Remove(plan);
            return await dbContext.SaveChangesAsync(ct);
        }

    }
}
