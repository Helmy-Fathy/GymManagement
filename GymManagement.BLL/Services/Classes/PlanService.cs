using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.PlanViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IGenericRepository<Membership> _membershipRepository;

        public PlanService(IGenericRepository<Plan> planRepository, IGenericRepository<Membership> membershipRepository)
        {
            _planRepository = planRepository;
            _membershipRepository = membershipRepository;
        }
        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _planRepository.GetAllAsync(ct: ct);
            return plans.Select(p => new PlanViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                IsActive = p.IsActive,
                Price = p.Price,
            });
        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(planId, ct);
            if (plan == null)
                return null;
            else
                return new PlanViewModel()
                {
                    Name = plan.Name,
                    Description = plan.Description,
                    Price = plan.Price,
                    DurationDays = plan.DurationDays,
                    IsActive = plan.IsActive,
                };
        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(planId, ct);
            if (plan is null || !plan.IsActive) return null;
            if (await HasActiveMembershipsAsync(planId, ct))
                return null;
            else
                return new UpdatePlanViewModel()
                {
                    PlanName = plan.Name,
                    Description = plan.Description,
                    Price = plan.Price,
                    DurationDays = plan.DurationDays
                };
        }

        public async Task<bool> UpdatePlanAsync(int planId, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(planId, ct);
            if (plan is null) return false;
            if (await HasActiveMembershipsAsync(planId, ct))
                return false;

            plan.DurationDays = model.DurationDays;
            plan.Price = model.Price;
            plan.Description = model.Description;
            plan.UpdatedAt = DateTime.Now;
            var result = await _planRepository.UpdateAsync(plan, ct);
            return result > 0;
        }

        public async Task<bool> ToggleActivationAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(planId, ct);
            if (plan is null) return false;

            if (plan.IsActive && await HasActiveMembershipsAsync(planId, ct))
                return false;

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;
            var result = await _planRepository.UpdateAsync(plan, ct);
            return result > 0;
        }



        private async Task<bool> HasActiveMembershipsAsync(int planId, CancellationToken ct = default)
        {
            return await _membershipRepository.AnyAsync(m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);
        }
    }
}
