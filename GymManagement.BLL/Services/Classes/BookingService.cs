using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.BookingViewModels;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSesssionsWithTrainerAndCategory(s => s.EndDate >= DateTime.Now, ct);
            var mappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - (await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct));
            }

            return mappedSessions;
        }

        public async Task<IEnumerable<MemberForSessionViewModel>> GetMembersForSessionAsync(int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetBySessionIdAsync(sessionId, ct);
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);
            return bookings.Select(b => new MemberForSessionViewModel
            {
                MemberId = b.MemberId,
                SessionId = b.SessionId,
                MemberName = b.Member.Name,
                BookingDate = $"{b.CreatedAt: MM/dd/yyy h:mm:ss tt}",
                IsAttended = session?.StartDate > DateTime.Now ? false : b.IsAttended
            }).ToList();
        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropdownAsync(int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetBySessionIdAsync(sessionId, ct);
            var bookedMemberIds = bookings.Select(b => b.MemberId);
            var availableMembers = await _unitOfWork.GetRepository<Member>().GetAllAsync(m => !bookedMemberIds.Contains(m.Id));

            return _mapper.Map<IEnumerable<MemberSelectListViewModel>>(availableMembers);
        }

        public async Task<Result> CreateNewBookingAsync(CreateBookingViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(model.SessionId, ct);
            if (session is null)
                return Result.NotFound("Session Is Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("You Cannot Book a Session That Alreadey Started");

            var membership = await _unitOfWork.MembershipRepository.AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now, ct);
            if (!membership)
                return Result.Fail("You Don't Have Active Membership");

            var alreadyBooked = await _unitOfWork.BookingRepository.AnyAsync(b => b.MemberId == model.MemberId && b.SessionId == model.SessionId, ct);
            if (alreadyBooked)
                return Result.Fail("You already booked this session");

            var bookedSlots = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(model.SessionId, ct);
            if (bookedSlots >= session.Capacity)
                return Result.Fail("Session is Full Capacity");

            var booking = new Booking()
            {
                MemberId = model.MemberId,
                SessionId = model.SessionId,
                IsAttended = false,
                CreatedAt = DateTime.Now
            };

            _unitOfWork.BookingRepository.Add(booking);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to book this session");

        }

        public async Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);
            if (session is null)
                return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot cancel Booking For a Session that has Already Started");

            var booking = await _unitOfWork.BookingRepository.FirstOrDefaultAsync(b => b.MemberId == memberId && b.SessionId == sessionId, true, ct);
            if (booking is null)
                return Result.NotFound("Booking Not Found");

            _unitOfWork.BookingRepository.Delete(booking);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed To Cancel Booking");
        }

        public async Task<Result> MarkAsAttendedAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.BookingRepository.FirstOrDefaultAsync(b => b.MemberId == memberId && b.SessionId == sessionId, true, ct);
            if (booking is null)
                return Result.NotFound("Booking Not Found");

            booking.IsAttended = true;
            booking.UpdatedAt = DateTime.Now;
            _unitOfWork.BookingRepository.Update(booking);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed To MarkAsAttended");
        }
    }
}
