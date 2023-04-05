using Domain.Entities.Secondary;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Account : IdentityUser<int>
    {

       
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [JsonIgnore]
        public  ICollection<Animal> Animals { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
      
           
            public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
       


    }
}
