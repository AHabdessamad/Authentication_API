using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> RegisterUserAsync(User user);

        Task<IEnumerable<User>> GetAllUserAsync();
        Task<IEnumerable<User>> SearchByUsername(string username);
        Task<bool> DeleteUser(int id);
    }
}
