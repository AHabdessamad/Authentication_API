using AutoMapper;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories;
using Service.Dtos;
using Service.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);

        }
        public async Task<UserDto> RegisterUserAsync(UserDto user)
        {
            //check if the user exist
            var existingUser = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                throw new Exception("user already exists");
            }

            var _user = await _userRepository.RegisterUserAsync(_mapper.Map<User>(user));

            return _mapper.Map<UserDto>(_user);
        }


        public bool VerifyPassword(User user, string password)
        {
            return new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, password) != PasswordVerificationResult.Failed;

        }

}
}


