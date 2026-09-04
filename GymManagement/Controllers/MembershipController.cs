using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.Design;

namespace GymManagement.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;
        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }
        public async Task<IActionResult> Index()
        {
            var memberships = await _membershipService.GetAllMembershipsAsync();
            return View(memberships);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct = default)
        {
            await PopulateDropdownList(ct);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMembershipViewModel model, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownList(ct);
                return View(model);
            }

            var result = await _membershipService.CreateMembershipAsync(model);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Membership Created Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.error;
            await PopulateDropdownList(ct);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id, CancellationToken ct = default)
        {
            var result = await _membershipService.DeleteActiveMembership(id, ct);
            TempData[result.success ? "SuccessMessage" : "ErrorMessage"] = result.success ? "Membership Cancelled" : result.error;
            return RedirectToAction(nameof(Index));  
        }

        private async Task PopulateDropdownList(CancellationToken ct = default)
        {
            var members = await _membershipService.GetMembersForDropdownListAsync(ct);
            var plans = await _membershipService.GetPlansForDropdownListAsync(ct);
            ViewBag.Members = new SelectList(members, "Id", "Name");
            ViewBag.Plans = new SelectList(plans, "Id", "Name");
        }
    }
}
