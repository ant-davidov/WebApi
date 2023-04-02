using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class RegistrationDTO
    {
        private string email;

        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        [MaxLength(200)]
        [Required(AllowEmptyStrings = false)]
        public string Email { get => email; set => email = value?.ToLower().Trim(); }
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}