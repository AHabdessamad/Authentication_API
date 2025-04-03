using AuthenticationAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos;
using Service.Services;
using Service.Services.Interfaces;

namespace WebAPI.Controllers
{

    [ApiController]
    [Route("/api/Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, IJwtService jwtService)
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

            Console.WriteLine(model);

            //var _user = _mapper.Map<UserDto>(model);
            var _user = new UserDto
            {
                Username = model.Username,
                Email = model.Email,
            };

            return _userService.RegisterUserAsync(_user) != null
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

            if (_user == null || !_userService.VerifyPassword(_user, model.Password))
                return BadRequest("Worng Password");
            

            var token = _jwtService.GenerateToken(_user.Id);

            return _userService.GetUserByUsernameAsync(model.Username) != null
                ? Ok(new { token })
                : BadRequest(new { message = "login failed" });

        }
    }

}
