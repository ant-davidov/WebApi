using Domain.Enums;

namespace Domain.DTOs
{
    public class AccountDTO
    {
        private string email;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get => email; set => email = value?.ToLower(); }
        public string Role { get; set; }
}
}
