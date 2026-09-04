using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly GymDbContext _dbContext;

        public BookingRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Booking>> GetBySessionIdAsync(int sessionId, CancellationToken ct = default)
        {
            var bookings = await _dbContext.Bookings.AsNoTracking()
                .Where(b => b.SessionId == sessionId)
                .Include(b => b.Member).ToListAsync(ct); 
            return bookings;
        }
    }
}
