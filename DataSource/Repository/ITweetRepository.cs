using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource.Repository
{
    public interface ITweetRepository
    {
        IEnumerable<Tweet> GetAllTweet();
        IEnumerable<Tweet> GetAllTweetFromUser(string username);
        Tweet CreateTweet(Tweet tweet);
        Tweet SearchTweetById(int id);
        void UpdateTweetWithNewData(Tweet tweet);
        Tweet DeleteTweet(Tweet tweet);
    }
}
