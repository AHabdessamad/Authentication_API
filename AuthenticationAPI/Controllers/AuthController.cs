using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos;
using Service.Services;

namespace WebAPI.Controllers
{
    public record RegisterModel(string Username, string Email, string Password);
    public record LoginModel(string Username, string Password);

    [ApiController]
    [Route("/api/Auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtService _jwtService;

        public AuthController(UserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = new PasswordHasher<User>().HashPassword(null, model.Password)
            };

            return _userService.RegisterUserAsync(user) != null
                ? Ok(new { message = "registered successed" })
                : BadRequest(new { message = "registration failed" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _user = await _userService.GetUserByUsernameAsync(model.Username);

            if (_user == null || new PasswordHasher<User>().VerifyHashedPassword(_user, _user.PasswordHash, model.Password) != PasswordVerificationResult.Failed)
                return BadRequest("Worng Password");

            var token = _jwtService.GenerateToken(_user.Id);

            return _userService.GetUserByUsernameAsync(model.Username) != null
                ? Ok(new { token })
                : BadRequest(new { message = "login failed" });

        }
    }

}
