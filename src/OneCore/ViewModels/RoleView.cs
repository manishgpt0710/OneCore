using System.ComponentModel.DataAnnotations;

namespace OneCore.Models
{
    public class RoleView
    {
        [Required]  
        public string Role { get; set; }
    }
}
