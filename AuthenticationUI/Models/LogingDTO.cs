using System.ComponentModel.DataAnnotations;

namespace AuthenticationUI.Models
{
    public enum role
    {
        User,
        Admin
    }   
    public class LogingDTO
    {
        [Required(ErrorMessage ="Username is required")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; } = string.Empty;
        public role Role { get; set; } = role.User;
    }
}
