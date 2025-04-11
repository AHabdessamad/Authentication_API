using AutoMapper;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories;
using Service.Dtos;
using Service.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using AuthenticationAPI.Models;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext context, IUserRepository userRepository, IMapper mapper)
        {
            _context = context;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);

        }

        public async Task<UserDto> RegisterUserAsync(RegisterModel user)
        {
            //var _user = new User { Email = user.Email, Username = user.Username, PasswordHash = user.Password };
            var _user = _mapper.Map<User>(user);

            _user.PasswordHash = new PasswordHasher<User>().HashPassword(_user, _user.PasswordHash);
            _user.Role = DAL.Entities.Role.User;

            var __user = await _userRepository.RegisterUserAsync(_user);

            return _mapper.Map<UserDto>(__user);

        }


        public bool VerifyPassword(User user, string password)
        {
            return new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, password) != PasswordVerificationResult.Failed;

        }

        public async Task<IEnumerable<UserDto>> GetAllUserAsync()
        {
            var users = await _userRepository.GetAllUserAsync();
            var _users = _mapper.Map<IEnumerable<UserDto>>(users);
            return _users;
        }

        public async Task<IEnumerable<UserDto>> SearchByUsername(string username)
        {
            var users = await _userRepository.SearchByUsername(username);
            var _users = _mapper.Map<IEnumerable<UserDto>>(users);
            return _users;
        }
    }
}


