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
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        [Required(AllowEmptyStrings = false)]
        public string Email { get => email; set => email = value?.ToLower().Trim(); }
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
      
    }
}
