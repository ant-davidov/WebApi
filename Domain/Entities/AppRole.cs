using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{

    public class AppRole : IdentityRole<int>
    {
        [Key]
        public override int Id { get; set; }
        public ICollection<AppAccountRole> UserRoles { get; set; }
    }

}
