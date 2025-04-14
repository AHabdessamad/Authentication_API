namespace AuthenticationUI.Models
{
    public enum Role { Admin, User};
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Email { get; set; } = default!;
        public Role? Role { get; set; }
        public string? Token { get; set; } = default!;
    }
}
