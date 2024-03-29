﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Secondary
{
  public class ApplicationRole : IdentityRole<int>
{
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
