using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MembershipViewModels
{
    public class CreateMembershipViewModel
    {
        public int MemberId { get; set; }
        public int PlanId { get; set; }
        public DateTime? StartDate { get; set; } 
    }
}
