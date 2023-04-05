using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities.Secondary
{
    public class ApplicationUserRole : IdentityUserRole<int>
    {
        [JsonIgnore]
        public virtual Account User { get; set; }
        [JsonIgnore]
        public virtual ApplicationRole Role { get; set; }
    }

}
