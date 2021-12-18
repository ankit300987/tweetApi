using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSource.Repository
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllUsersAsyc();
        public Task<string> RegisterUserAsync(User user);
        public Task<User> SearchUserAsync(string username);
        public Task<User> GetUserByIdAsync(int id);
    }
}