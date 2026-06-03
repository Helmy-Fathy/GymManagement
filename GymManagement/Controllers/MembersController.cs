using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService )
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

        //GET BaseUrl/Members/HealthRecordDeatails/{id}
        //HealthRecordDeatails - show one member's HealthRecord details

        #region Create Member 
        //GET BaseUrl/Members/Create
        //Create - show empty form

        //POST BaseUrl/Members/Create {member}
        //Create - Submit form 
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
