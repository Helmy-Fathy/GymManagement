using GymManagement.Configurations;
using GymManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DbContexts
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options) 
        {
            
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=GymManagementDb;Trusted_Connection=true;TrustServerCertificate=true");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Plan>(new PlanConfiguration());
        }
        public DbSet<Plan> Plans { get; set; }
    }
}
