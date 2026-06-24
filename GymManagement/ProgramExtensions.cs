using GymManagement.DAL.Data.DataSeeding;
using GymManagement.DAL.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.PL
{
    public static class ProgramExtensions
    {
        public static async Task MigrateAndSeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var pendingMigrations = await dbContext.Database.GetAppliedMigrationsAsync();

            if (pendingMigrations.Any())
            {
                logger.LogInformation($"Applying {pendingMigrations.Count()} Pending Migrations");
                await dbContext.Database.MigrateAsync();
            }

            var seedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeeding.SeedAsync(dbContext, seedFolderPath, logger);
        }
    }
}
