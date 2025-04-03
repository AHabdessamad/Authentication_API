using DAL.Entities;
using Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetUserByUsernameAsync(string username);
        public Task<UserDto> RegisterUserAsync(User user);
    }
}
