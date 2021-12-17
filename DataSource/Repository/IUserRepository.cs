using Core.Models;
using System.Collections.Generic;

namespace DataSource
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        string RegisterUser(User user);
        User SearchUser(string username);
        User GetUserById(int id);
    }
}