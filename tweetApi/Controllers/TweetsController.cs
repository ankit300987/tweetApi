using Core.Models;
using DataSource.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tweetApi.Controllers
{
    [ApiController]
    [Route("api/v1.0/[controller]")]
    public class TweetsController : ControllerBase
    {
        private readonly ITweetRepository TweetRepository;
        private readonly ILogger<TweetsController> Logger;
        public TweetsController(ITweetRepository tweetRepository, ILogger<TweetsController> logger)
        {
            TweetRepository = tweetRepository;
            this.Logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> AllAsync()
        {
            try
            {
                var tweets = await TweetRepository.GetAllTweetAsync();
                var replyTweets = tweets.Where(x => x.ParentId.HasValue && x.ParentId != 0).ToList();
                foreach (var tweet in tweets)
                {
                    var replyTweet = replyTweets.Where(x => x.ParentId == tweet.Id).ToList();
                    tweet.Reply = replyTweet;
                }
                var viewTweets = tweets.Where(t => t.ParentId.GetValueOrDefault() == 0).ToList();
                Logger.LogInformation("Getting All tweets");
                return Ok(viewTweets);
            }
            catch (Exception ex)
            {
                Logger.LogError("Getting Exception from TweetsController.All(), while fetching all tweets.", ex.Message);
                return StatusCode(500, "Server Error while Getting all tweets");
            }
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetTweetsOfUserNameAsync(string username)
        {
            try
            {
                var tweets = await TweetRepository.GetAllTweetFromUserAsync(username);
                Logger.LogInformation("Receiving all tweets of the user " + username);
                return Ok(tweets);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Getting Exception from TweetsController.GetTweetsOfUserName({username}), while fetching all tweets for the user {username}.", ex.Message);
                return StatusCode(500, "Server Error while Getting all tweets of user");
            }
        }

        [HttpPost]
        [Route("/api/v1.0/tweets/{username}/add")]
        public async Task<IActionResult> CreateTweetAsync([FromBody] Tweet tweet)
        {
            try
            {
                Tweet newtweet = await TweetRepository.CreateTweetAsync(tweet);
                Logger.LogInformation($"Post a new tweet for user {tweet.UserName}");
                return Ok(newtweet);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Getting Exception from TweetsController.CreateTweet(), while posting tweet.", ex.Message);
                return StatusCode(500, "Server Error while while posting tweet.");
            }
        }

        [HttpGet]
        [Route("/api/v1.0/tweets/{username}/search/{id}")]
        public async Task<IActionResult> SearchTweetByIdAsync(int id)
        {
            try
            {
                Tweet tweet = await TweetRepository.SearchTweetByIdAsync(id);
                Logger.LogInformation($"Searching Tweet with {id} in the db");
                return tweet != null ? Ok(tweet) : NotFound($"Tweet with id {id} was not found in the db");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Getting Exception from TweetsController.SearchTweetById({id}), while searching tweet.", ex.Message);
                return StatusCode(500, "Server Error while searching tweet for id");
            }
        }

        [HttpPut]
        [Route("/api/v1.0/tweets/{username}/update/{id}")]
        public async Task<IActionResult> UpdateTweetAsync([FromBody] Tweet tweet, [FromRoute] int id, [FromRoute] string username)
        {
            try
            {
                if (id != tweet.Id && username != tweet.UserName) return BadRequest($"Tweet id {id} for the user {username} is not matching with tweets data {tweet.Id} & {tweet.UserName}");
                var searchedTweet = await TweetRepository.SearchTweetByIdAsync(id);

                if (searchedTweet == null) return NotFound();

                await TweetRepository.UpdateTweetWithNewDataAsync(tweet);
                Logger.LogInformation($"Tweet with id {tweet.Id} gets updated");
                return Ok($"Updating post with id {id} for user {username} while tweet data has {tweet.Id} & {tweet.UserName}");

            }
            catch (Exception ex)
            {
                Logger.LogError($"Getting Exception from TweetsController.UpdateTweet(tweet, {id},{username}), while updating tweet.", ex.Message);
                return StatusCode(500, "Server Error while updating tweet for id");
            }
        }

        [HttpDelete]
        [Route("/api/v1.0/tweets/{username}/delete/{id}")]
        public async Task<IActionResult> DeleteTweetAsync(string username, int id)
        {
            try
            {
                Tweet tweet = await TweetRepository.SearchTweetByIdAsync(id);
                if (tweet == null || tweet?.UserName != username)
                {
                    Logger.LogWarning($"Tweet with tweet id {id} was not found for user {username}");
                    return NotFound();
                }

                Tweet deletedTweet = await TweetRepository.DeleteTweetAsync(tweet);
                Logger.LogInformation($"Deleting post with tweet id {tweet.Id} for user {tweet.UserName}");
                return Ok(deletedTweet);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Getting Exception from TweetsController.DeleteTweet({username},{id}), while deleting tweet.", ex.Message);
                return StatusCode(500, "Server Error while deleting tweet for id");
            }
        }

        /// <summary>
        /// The user having username likes other's tweet having id
        /// </summary>
        /// <param name="username"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("/api/v1.0/tweets/{username}/like/{id}")]
        public async Task<IActionResult> LikeTweetAsync([FromRoute] string username, [FromRoute] int id)
        {
            try
            {
                Tweet searchedTweet = await TweetRepository.SearchTweetByIdAsync(id);
                if (searchedTweet == null)
                {
                    Logger.LogWarning($"Tweet with tweet id {id} was not found");
                    return NotFound();
                }
                if (username.ToLower() == searchedTweet.UserName?.ToLower()) return BadRequest("Like User and Tweet user can not be same");

                List<string> likesBy = searchedTweet.LikesBy?.ToList() ?? new List<string>();
                if (!likesBy.Where(x => x == username).Any())
                    likesBy.Add(username);
                else
                    likesBy.Remove(username);


                searchedTweet.LikesBy = likesBy;
                await TweetRepository.UpdateTweetWithNewDataAsync(searchedTweet);
                Logger.LogInformation($"Like post with tweet id {id} for user {username}");
                return Ok($"Like post with tweet id {id} for user {username}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Getting Exception from TweetsController.LikeTweet({username},{id}), while liking tweet.", ex.Message);
                return StatusCode(500, "Server Error while liking tweet for id");
            }
        }

        /// <summary>
        /// Reply given by user having username to the other user's tweet having id 
        /// </summary>
        /// <param name="tweet"></param>
        /// <param name="username"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/v1.0/tweets/{username}/reply/{id}")]
        public async Task<IActionResult> ReplyTweetAsync([FromBody] Tweet tweet, string username, int id)
        {
            try
            {
                Tweet searchedTweet = await TweetRepository.SearchTweetByIdAsync(id);
                if (searchedTweet == null)
                {
                    Logger.LogWarning($"Tweet with tweet id {id} was not found for user {username}");
                    return NotFound();
                }
                tweet.ParentId = searchedTweet.Id;
                Tweet newtweet = await TweetRepository.CreateTweetAsync(tweet);
                var reply = searchedTweet.Reply?.ToList() ?? new List<Tweet>();
                reply.Add(newtweet);
                searchedTweet.Reply = reply;
                await TweetRepository.UpdateTweetWithNewDataAsync(searchedTweet);
                Logger.LogInformation($"Post a reply tweet for user {username}");
                return Ok($"Replying post {tweet.Id} for user {username}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Getting Exception from TweetsController.ReplyTweet(), while replying tweet.", ex.Message);
                return StatusCode(500, "Server Error while while replying tweet.");
            }
        }
    }
}
