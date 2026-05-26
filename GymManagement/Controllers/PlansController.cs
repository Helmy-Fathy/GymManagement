using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanRepository _planRepository;

        public PlansController( IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planRepository.GetAllAsync(ct: ct);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id , CancellationToken ct) 
        { 
            var plan = await _planRepository.GetByIdAsync(id,ct);
            if(plan is null)
                return RedirectToAction(nameof(Index));
            else 
                return View(plan);
        } 
    }
}
