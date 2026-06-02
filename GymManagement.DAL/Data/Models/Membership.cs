using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Models
{
    public class Membership : BaseEntity
    {
        public Member Member { get; set; }
        public int MemberId { get; set; }

        public Plan Plan  { get; set; }
        public int PlanId { get; set; }

        // StartDate = CreatedAt Of BaseEntity
        public DateTime EndDate { get; set; }

        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
        public bool IsActive => EndDate > DateTime.Now;
        
    }
}
