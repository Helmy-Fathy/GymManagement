using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        // GET BaseUrl/Members/Index
        // Index - List all members
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAllMembersAsync(ct);
            return View(members);
        }

        //GET BaseUrl/Members/MemberDeatails/{id}
        //MemberDetails - show one member's details
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {

            var member = await _memberService.GetMemberDetailsByIdAsync(id , ct);

            if(member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        //GET BaseUrl/Members/HealthRecordDeatails/{id}
        //HealthRecordDeatails - show one member's HealthRecord details
        //public IActionResult HealthRecordDeatails(int id, CancellationToken ct)
        //{
        //    // Get Health Record By member Id
        //    // Check Is Health Record Null => Return Index With Message
        //    // Health Record  Is Not Null => Return View Data
        //}


        #region Create Member 
        //GET BaseUrl/Members/Create
        //Create - show empty form
        [HttpGet]
        public IActionResult Create() => View();


        //POST BaseUrl/Members/Create {member}
        //CreateMember - Submit form 
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);

            var result = await _memberService.CreateMemberAsync(model, ct);
            if (result)
                TempData["SuccessMessage"] = "Member Created Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Create Member";

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit Member
        //GET BaseUrl/Members/Edit/{id}
        //EditMember - Display edit form

        //POST BaseUrl/Members/Edit {member}
        //Edit - Submit form  
        #endregion

        #region Delete Member 
        //GET BaseUrl/Members/Delete/{id}
        //Delete - show confirmation form

        //POST BaseUrl/Members/DeleteConfirmed/{id}
        //DeleteConfirmed - Submit form  
        #endregion

    }
}
