using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIExecutor.APIClient;

namespace WebAPIExecutor
{
    public class FetchTweetData
    {
        private readonly IWebAPIExecutor webAPIExecutor;

        public FetchTweetData(IWebAPIExecutor webAPIExecutor)
        {
            this.webAPIExecutor = webAPIExecutor;
        }

        public async Task<IEnumerable<Tweet>> GetTweets()
        {
            return await webAPIExecutor.InvokeGetAsync<IEnumerable<Tweet>>("/api/v1.0/tweets");
        }

        public async Task<IEnumerable<Tweet>> GetTweetsFromUser(string username)
        {
            return await webAPIExecutor.InvokeGetAsync<IEnumerable<Tweet>>($"/api/v1.0/Tweets/{username}");
        }

        public async Task<Tweet> SeaarchTweetById(string username, int id)
        {
            return await webAPIExecutor.InvokeGetAsync<Tweet>($"/api/v1.0/Tweets/{username}/search/{id}");
        }

        public async Task<Tweet> CreatePost(Tweet tweet, string username)
        {
            return await webAPIExecutor.InvokePostAsync<Tweet>($"/api/v1.0/tweets/{username}/add ", tweet);
        }

        public async Task<Tweet> ReplyPost(Tweet tweet, string username, int tweetId)
        {
            return await webAPIExecutor.InvokePostAsync<Tweet>($"/api/v1.0/tweets/{username}/reply/{tweetId} ", tweet);
        }

        public async Task UpdatePost(Tweet tweet, string username, int tweetId)
        {
            await webAPIExecutor.InvokePutAsync($"/api/v1.0/tweets/{username}/update/{tweetId} ", tweet);
        }

        public async Task LikePost(Tweet tweet, string username, int tweetId)
        {
            await webAPIExecutor.InvokePutAsync($"/api/v1.0/tweets/{username}/like/{tweetId} ", tweet);
        }

        public async Task DeletePost(string username, int tweetId)
        {
            await webAPIExecutor.InvokeDeleteAsync<Tweet>($"/api/v1.0/tweets/{username}/like/{tweetId}");
        }
    }
}
