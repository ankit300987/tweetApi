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
        public Task<Tweet> CreateTweetAsync(Tweet tweet);
        public Task<Tweet> DeleteTweetAsync(Tweet tweet);
        public Task<IEnumerable<Tweet>> GetAllTweetAsync();
        public Task<IEnumerable<Tweet>> GetAllTweetFromUserAsync(string username);
        public Task<Tweet> SearchTweetByIdAsync(int id);
        public Task UpdateTweetWithNewDataAsync(Tweet tweet);
    }
}
