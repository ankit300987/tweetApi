using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<IEnumerable<User>> GetAllUsersAsyc()
        {
            return await db.Users.AsNoTracking().ToListAsync();
        }

        
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await db.Users.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> LoginUserAsync(string userName, string password)
        {
            User user = await SearchUserAsync(userName);
            if (user == null) return null;
            if (user.Password != password) return null;
            return user;
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
            return db.Users.AsNoTracking().Where(x => x.UserName == username).FirstOrDefault();
        }

        public async Task<User> SearchUserAsync(string username)
        {
            return await db.Users.AsNoTracking().Where(x => x.UserName == username).FirstOrDefaultAsync();
        }

        public async  Task UpdateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user), "User is null");
            //db.Tweets.Add(tweet);
            db.Entry(user).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}