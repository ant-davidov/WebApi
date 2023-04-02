using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppAccountRole : IdentityUserRole<int>
    {
      
        public int Id { get; set; } 
        public Account User { get; set; }
        public AppRole Role { get; set; }
    }
}
