using AutoMapper;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories;
using Service.Dtos;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }
        public async Task<UserDto> RegisterUserAsync(User user)
        {
            //check if the user exist
            var existingUser = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                throw new Exception("user already exists");
            }
            
            var _user = await _userRepository.RegisterUserAsync(user);

            return _mapper.Map<UserDto>(_user);
        }
    }
}


