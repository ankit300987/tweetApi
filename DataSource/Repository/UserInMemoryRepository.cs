using Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataSource
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

        public User GetUserById(int id)
        {
            return db.Users.Where(x => x.Id == id).FirstOrDefault();
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


        public User SearchUser(string username)
        {
            return db.Users.Where(x => x.UserName == username).FirstOrDefault();
        }

    }
}