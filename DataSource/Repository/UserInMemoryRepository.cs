using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSource.Repository
{
    public class UserInMemoryRepository : IUserRepository
    {
        private readonly TweetAppContext db;

        public UserInMemoryRepository(TweetAppContext db)
        {
            this.db = db;
        }
        public IEnumerable<User> GetAllUsers()
        {
            return db.Users.ToList();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsyc()
        {
            return await db.Users.ToListAsync();
        }

        public User GetUserById(int id)
        {
            return db.Users.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public string RegisterUser(User user)
        {
            var searchedUser = SearchUser(user.UserName);
            if (searchedUser != null)
            {
                return $"User with username {user.UserName } is already existed";
            }
            ///Create User In DB
            ///
            db.Users.Add(user);
            db.SaveChanges();
            return null;
        }

        public async Task<string> RegisterUserAsync(User user)
        {
            var searchedUser = await SearchUserAsync(user.UserName);
            if (searchedUser != null)
            {
                return $"User with username {user.UserName } is already existed";
            }
            ///Create User In DB
            ///
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return null;
        }

        public User SearchUser(string username)
        {
            return db.Users.Where(x => x.UserName == username).FirstOrDefault();
        }

        public async Task<User> SearchUserAsync(string username)
        {
            return await db.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
        }
    }
}