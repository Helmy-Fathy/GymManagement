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
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        private readonly GymDbContext _dbContext;

        public MembershipRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Membership>> GetMembershipsWithMemberAndPlan(Expression<Func<Membership, bool>>? predicate = null, CancellationToken ct = default)
        {
            var query = _dbContext.Memberships.Include(m => m.Member).Include(m => m.Plan).AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            return await query.ToListAsync(ct);         

        }
    }
}
