using GymManagement.DAL.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MemberViewModels
{
    public class CreateMemberViewModel
    {
        [Required(ErrorMessage ="Name Is Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$",ErrorMessage ="Name can only cotain letters and spaces")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage ="Email Is Required")]
        [EmailAddress(ErrorMessage ="Invaild Email Format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Phone Number Is Required")]
        [Phone(ErrorMessage ="Invaild Phone Number")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage ="Phone Number must be  a valid Egyptian number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = default!;

        [Required(ErrorMessage ="Date of Birth is Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender Is Required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Building Number Is Required")]
        [Range(1,9000,ErrorMessage = "Building Number must be Greater than 0")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage ="City is Required")]
        [StringLength(100,MinimumLength =2 , ErrorMessage ="City must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can only cotain letters and spaces")]
        public string City { get; set; } = default!;

        [Required(ErrorMessage = "Street is Required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 150 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Street can only cotain letters and spaces")]
        public string Street { get; set; } = default!;

        [Required(ErrorMessage = "Health record is Required")]

        public HelathRecordViewModel HelathRecordViewModel { get; set; } = default!;
    }
}
