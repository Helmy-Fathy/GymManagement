using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.BookingViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            return View(await _bookingService.GetAllSessionsAsync(ct));
        }

        [HttpGet]
        public async Task<IActionResult> GetMembersForUpComingSession(int id, CancellationToken ct)
        {
            return View(await _bookingService.GetMembersForSessionAsync(id, ct));
        }

        [HttpGet]
        public async Task<IActionResult> GetMembersForOngoingSession(int id, CancellationToken ct)
        {
            return View(await _bookingService.GetMembersForSessionAsync(id, ct));
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id, CancellationToken ct)
        {
            var members = await _bookingService.GetMembersForDropdownAsync(id, ct);
            ViewBag.Members = new SelectList(members, "Id", "Name");
            ViewBag.SessionId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingViewModel model, CancellationToken ct)
        {
            var result = await _bookingService.CreateNewBookingAsync(model, ct);
            TempData[result.success ? "Success" : "Error Message"] = result.success ? "Booking Created Successfully.." : result.error;
            return RedirectToAction(nameof(GetMembersForUpComingSession), new { id = model.SessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int memberId, int sessionId,CancellationToken ct)
        {
            var result = await _bookingService.CancelBookingAsync(memberId, sessionId, ct);
            TempData[result.success ? "Success" : "Error Message"] = result.success ? "Booking Cancelled Successfully.." : result.error;
            return RedirectToAction(nameof(GetMembersForUpComingSession), new { id = sessionId});
        }

        [HttpPost]
        public async Task<IActionResult> Attended(int memberId, int sessionId, CancellationToken ct)
        {   
            var result = await _bookingService.MarkAsAttendedAsync(memberId, sessionId, ct);
            TempData[result.success ? "Success" : "Error Message"] = result.success ? "Attended Recorded Successfully.." : result.error;
            return RedirectToAction(nameof(GetMembersForOngoingSession), new { id = sessionId });
        }


    }
}
