using GymManagement.BLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);

        Task<PlanViewModel?> GetPlanByIdAsync(int planId, CancellationToken ct = default);

        Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default);

        Task<bool>UpdatePlanAsync(int planId, UpdatePlanViewModel model , CancellationToken ct = default);

        Task<bool> ToggleActivationAsync(int planId, CancellationToken ct = default);
    }
}
