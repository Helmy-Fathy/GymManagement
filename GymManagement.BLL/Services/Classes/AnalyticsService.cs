using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.AnalyticsViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;

namespace GymManagement.BLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AnalyticsViewModel> GetDataAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var upcomingSessions = await _unitOfWork.GetRepository<Session>().CountAsync(s => s.StartDate > now, ct);
            var ongoingSessions = await _unitOfWork.GetRepository<Session>().CountAsync(s => s.StartDate <= now && s.EndDate >= now, ct);
            var completedSessions = await _unitOfWork.GetRepository<Session>().CountAsync(s => s.EndDate < now, ct);
            var totalMembers = await _unitOfWork.GetRepository<Member>().CountAsync(ct: ct);
            var totalTrainer = await _unitOfWork.GetRepository<Trainer>().CountAsync(ct: ct);
            var activeMembers = await _unitOfWork.GetRepository<Membership>().CountAsync(x => x.EndDate > now, ct);

            return new AnalyticsViewModel()
            {
                TotalMembers = totalMembers,
                TotalTrainers = totalTrainer,
                ActiveMembers = activeMembers,
                UpcomingSessions = upcomingSessions,
                OngoingSessions = ongoingSessions,
                CompletedSessions = completedSessions
            };
        }
    }
}
