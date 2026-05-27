using GymManagement.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class Trainer : GymUser
    {
        //HireDate = CreatedAt Of BaseEntity
        public Speciality Speciality { get; set; }
    }
}
