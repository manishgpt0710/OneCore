
using System.ComponentModel.DataAnnotations;

namespace OneCore.Models
{
    public class UserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Address { get; set; }
    }
}
