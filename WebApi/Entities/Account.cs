using System.ComponentModel.DataAnnotations;

namespace WebApi.Entities
{
    public class Account
    {
        private string email;

        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Email { get => email; set => email = value?.ToLower(); }
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
