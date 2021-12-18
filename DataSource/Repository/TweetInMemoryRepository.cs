using Core.Models;
using DataSource.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSource
{
    public class TweetInMemoryRepository : ITweetRepository
    {
        private readonly TweetAppContext db;

        public TweetInMemoryRepository(TweetAppContext db)
        {
            this.db = db;
        }

        public async Task<Tweet> CreateTweetAsync(Tweet tweet)
        {
            if (tweet == null) throw new ArgumentNullException(nameof(tweet), "Tweet is null");
            await db.Tweets.AddAsync(tweet);
            await db.SaveChangesAsync();
            return tweet;
        }

        public async Task<Tweet> DeleteTweetAsync(Tweet tweet)
        {
            db.Tweets.Remove(tweet);
            await db.SaveChangesAsync();
            return tweet;
        }

        public async Task<IEnumerable<Tweet>> GetAllTweetAsync()
        {
            return await db.Tweets.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Tweet>> GetAllTweetFromUserAsync(string username)
        {
            return username != null ? await db.Tweets.Where(t => t.UserName == username).AsNoTracking().ToListAsync() : new List<Tweet>();
        }

        public async Task<Tweet> SearchTweetByIdAsync(int id)
        {
            return await db.Tweets.Where(t => t.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task UpdateTweetWithNewDataAsync(Tweet tweet)
        {
            if (tweet == null) throw new ArgumentNullException(nameof(tweet), "Tweet is null");
            //db.Tweets.Add(tweet);
            db.Entry(tweet).State = EntityState.Modified;
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
