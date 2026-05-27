using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class Category : BaseEntity
    {
        public string CategryName { get; set; } = default!;
    }
}
