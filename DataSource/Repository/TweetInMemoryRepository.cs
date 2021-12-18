using Core.Models;
using DataSource.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSource
{
    public class TweetInMemoryRepository : ITweetRepository
    {
        private readonly TweetAppContext db;

        public TweetInMemoryRepository(TweetAppContext db)
        {
            this.db = db;
        }
        public Tweet CreateTweet(Tweet tweet)
        {
            if (tweet == null) throw new ArgumentNullException(nameof(tweet), "Tweet is null");
            db.Tweets.Add(tweet);
            db.SaveChanges();
            return tweet;
        }

        public Tweet DeleteTweet(Tweet tweet)
        {
            db.Tweets.Remove(tweet);
            db.SaveChanges();
            return tweet;
        }

        public IEnumerable<Tweet> GetAllTweet()
        {
            return db.Tweets.ToList();
        }

        public IEnumerable<Tweet> GetAllTweetFromUser(string username)
        {
            return username != null? db.Tweets.Where(t => t.UserName == username).ToList(): new List<Tweet>(); 
        }

        public Tweet SearchTweetById(int id)
        {
            return db.Tweets.Where(t => t.Id == id).FirstOrDefault();
        }

        public void UpdateTweetWithNewData(Tweet tweet)
        {
            if (tweet == null) throw new ArgumentNullException(nameof(tweet), "Tweet is null");
            //db.Tweets.Add(tweet);
            db.Entry(tweet).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
