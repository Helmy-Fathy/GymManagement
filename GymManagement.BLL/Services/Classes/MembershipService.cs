using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default)
        {
            var memberships = await _unitOfWork.MembershipRepository.GetMembershipsWithMemberAndPlan(m => m.EndDate > DateTime.Now, ct);
            return _mapper.Map<IEnumerable<MembershipViewModel>>(memberships);
        }

        public async Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default)
        {
            var memberExist = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Id == model.MemberId, ct);
            if (!memberExist)
                return Result.NotFound("Memnber Not Found");

            var planExist = await _unitOfWork.GetRepository<Plan>().AnyAsync(p => p.Id == model.PlanId, ct);
            if (!planExist)
                return Result.NotFound("Plan Not Found");

            var hasActiveMembership = await _unitOfWork.MembershipRepository.AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now, ct);
            if (hasActiveMembership)
                return Result.Fail("The Member Has An Active Membership");

            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(model.PlanId, ct);
            if (!plan.IsActive)
                return Result.Fail("The Plan Is Not Active");

            //var membership = new Membership()
            //{
            //    Id = model.MemberId,
            //    PlanId = model.PlanId,
            //    CreatedAt = DateTime.Now,
            //    EndDate = (model.StartDate ?? DateTime.Now).AddDays(plan.DurationDays),
            //};
            var membership = _mapper.Map<Membership>(model);
            membership.EndDate = (model.StartDate ?? DateTime.Now).AddDays(plan.DurationDays);

            _unitOfWork.MembershipRepository.Add(membership);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            if (result > 0)
                return Result.Ok();
            else
                return Result.Fail("Failed To Create Membership");
        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropdownListAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);
        }

        public async Task<IEnumerable<PlanSelectListViewModel>> GetPlansForDropdownListAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<PlanSelectListViewModel>>(plans);
        }

        public async Task<Result> DeleteActiveMembership(int memberId, CancellationToken ct = default)
        {
            var ActiveMembership = await _unitOfWork.MembershipRepository.FirstOrDefaultAsync(m => m.MemberId == memberId && m.EndDate > DateTime.Now, true, ct);
            if (ActiveMembership == null)
                return Result.NotFound("Active Membership Not Found For The member");

            _unitOfWork.MembershipRepository.Delete(ActiveMembership);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            if (result > 0)
                return Result.Ok();
            else
                return Result.Fail("Failed To Cancel Membership");
        }
    }
}
