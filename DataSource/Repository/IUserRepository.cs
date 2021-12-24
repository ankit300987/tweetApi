using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSource.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsyc();
        Task<string> RegisterUserAsync(User user);
        Task<User> SearchUserAsync(string username);
        Task<User> GetUserByIdAsync(int id);
        Task UpdateUserAsync(User user);
        Task<User> LoginUserAsync(string userName, string password);
    }
}