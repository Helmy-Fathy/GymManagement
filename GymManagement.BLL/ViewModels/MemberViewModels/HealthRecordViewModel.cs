using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {
        [Range(0.1,300, ErrorMessage ="Height must be greater than 0")]
        public decimal Height { get; set; }

        [Range(0.1, 500, ErrorMessage = "weight must be greater than 0")]
        public decimal weight { get; set; }

        [Required(ErrorMessage ="Blood Type is required")]
        [StringLength(3,ErrorMessage ="Blood Type must be 3 characters or less")]
        public string BloodType { get; set; } = default!;
        public string? Note { get; set; } 
    }
}
