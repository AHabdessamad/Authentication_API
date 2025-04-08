using System.ComponentModel.DataAnnotations;

namespace AuthenticationUI.Models
{
    public class LogingDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
