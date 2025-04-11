using System.ComponentModel.DataAnnotations;

namespace AuthenticationUI.Models
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="Username is required")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="Invalid email address")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; } = string.Empty;
        public role Role { get; set; } = role.User;
    }
}
