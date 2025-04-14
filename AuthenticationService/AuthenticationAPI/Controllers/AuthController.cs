using AuthenticationAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos;
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
        private readonly ILogger _logger;

        public AuthController(IUserService userService, IJwtService jwtService, ILogger logger, IMapper mapper  )
        {
            _userService = userService;
            _jwtService = jwtService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _user = await _userService.GetUserByUsernameAsync(model.Username);
          

            if (_user != null) return BadRequest(new { message ="User already exist" });

            _logger.LogInformation("Registering to the application...");

            return _userService.RegisterUserAsync(model) != null
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
                return BadRequest("Worng password Or User not exist");

            var __user = _mapper.Map<UserDto>(_user);

            var token = _jwtService.GenerateToken(__user);


            return _user != null
                ? Ok(new { token })
                : BadRequest("login failed");

        }


        [HttpGet("user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var _user = await _userService.GetAllUserAsync();

            if (_user == null)
                return BadRequest("User not exist");

            return Ok( _user );
        }

        [HttpGet("user/search")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SerachByUsername(string username)
        {
            var _user = await _userService.SearchByUsername(username);

            if (_user == null)
                return BadRequest("User not exist");

            return Ok(new { _user });
        }

        [HttpDelete("user/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<bool> DeleteUser(int id)
        {
            var res = await _userService.DeleteUser(id);
            if (!res)
            {
                return false;
            }

            return true;
        }


    }

}
