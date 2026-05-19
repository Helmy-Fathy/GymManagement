using GymManagement.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {
        private readonly GymDbContext dbContext;

        public PlansController()
        {
            dbContext = new GymDbContext();
        }

        public async Task<IActionResult> Index()
        {
            var plans = await dbContext.Plans.ToListAsync();
            return View(plans);
        }

        public async Task<IActionResult> Details(int id) 
        { 
            var plan = await dbContext.Plans.FindAsync(id);
            if(plan is null)
                return RedirectToAction(nameof(Index));
            else 
                return View(plan);
        } 
    }
}
