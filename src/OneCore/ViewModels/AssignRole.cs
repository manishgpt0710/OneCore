using System.ComponentModel.DataAnnotations;

namespace OneCore.Models
{
    public class AssignRole
    {
        [Required]
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
