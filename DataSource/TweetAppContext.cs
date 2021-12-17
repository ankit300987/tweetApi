using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class TweetAppContext : DbContext
    {
        public TweetAppContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<Tweet> Tweets;
        public DbSet<User> Users;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tweet>()
                .HasMany(t => t.LikesBy);


            ///Seeding
            ///
            modelBuilder.Entity<User>().HasData(
                new User { Id=1 ,FirstName = "Test User ", LastName = "Last Name", EmailId = "test@test.com", UserName = "testUser", Password = "Test", ConfirmPassword = "Test" },
                new User { Id = 2,FirstName = "Test Second User ", LastName = "Last Name", EmailId = "test2@test.com", UserName = "testUser2", Password = "Test", ConfirmPassword = "Test" }
                ) ;
            modelBuilder.Entity<Tweet>().HasData(
                    new Tweet { Id=1, UserName="testUser",  LikesBy=new List<User>(),Message="Hello Seeded Data ", Name = "Test User Last Name", ParentId= null, TimeStamp= DateTime.Now}
                );
        }
    }
}
