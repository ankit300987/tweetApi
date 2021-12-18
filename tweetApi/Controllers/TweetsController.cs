using Core.Models;
using DataSource;
using DataSource.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public IActionResult All()
        {
            try
            {
                var tweets = TweetRepository.GetAllTweet();
                Logger.LogInformation("Getting All tweets");
                return Ok(tweets);
            }
            catch (Exception ex)
            {
                Logger.LogError("Getting Exception from TweetsController.All(), while fetching all tweets.", ex.Message);
                return StatusCode(500, "Server Error while Getting all tweets");
            }
        }

        [HttpGet("{username}")]
        public IActionResult GetTweetsOfUserName(string username)
        {
            try
            {
                var tweets = TweetRepository.GetAllTweetFromUser(username);
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
        public IActionResult CreateTweet([FromBody] Tweet tweet)
        {
            try
            {
                Tweet newtweet = TweetRepository.CreateTweet(tweet);
                Logger.LogInformation($"Post a new tweet for user {tweet.UserName}");
                return Ok($"Creating post {tweet} for user {tweet.UserName}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Getting Exception from TweetsController.CreateTweet(), while posting tweet.", ex.Message);
                return StatusCode(500, "Server Error while while posting tweet.");
            }
        }

        [HttpGet]
        [Route("/api/v1.0/tweets/{username}/search/{id}")]
        public IActionResult SearchTweetById(int id)
        {
            try
            {
                Tweet tweet = TweetRepository.SearchTweetById(id);
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
        public IActionResult UpdateTweet([FromBody] Tweet tweet, [FromRoute] int id, [FromRoute] string username)
        {
            try
            {
                if (id != tweet.Id && username != tweet.UserName) return BadRequest($"Tweet id {id} for the user {username} is not matching with tweets data {tweet.Id} & {tweet.UserName}");
                var searchedTweet = TweetRepository.SearchTweetById(id);

                if (searchedTweet == null) return NotFound();

                TweetRepository.UpdateTweetWithNewData(tweet);
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
        public IActionResult DeleteTweet(string username,int id)
        {
            try
            {
                Tweet tweet = TweetRepository.SearchTweetById(id);
                if (tweet == null || tweet?.UserName != username)
                {
                    Logger.LogWarning($"Tweet with tweet id {id} was not found for user {username}");
                    return NotFound();
                }

                Tweet deletedTweet = TweetRepository.DeleteTweet(tweet);
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
        public IActionResult LikeTweet([FromRoute]string username, [FromRoute]int id)
        {
            try
            {
                Tweet searchedTweet = TweetRepository.SearchTweetById(id);
                if (searchedTweet == null)
                {
                    Logger.LogWarning($"Tweet with tweet id {id} was not found");
                    return NotFound();
                }
                List<string> likesBy = searchedTweet.LikesBy.ToList();
                likesBy.Add(username);
                searchedTweet.LikesBy = likesBy;
                TweetRepository.UpdateTweetWithNewData(searchedTweet);
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
        public IActionResult ReplyTweet([FromBody] Tweet tweet, string username, int id)
        {
            try
            {
                Tweet searchedTweet = TweetRepository.SearchTweetById(id);
                if (searchedTweet == null || tweet?.UserName != username)
                {
                    Logger.LogWarning($"Tweet with tweet id {id} was not found for user {username}");
                    return NotFound();
                }
                tweet.ParentId = id;
                Tweet newTweet = TweetRepository.CreateTweet(tweet);
                Logger.LogInformation($"Post a reply tweet for user {newTweet.UserName}");
                return Ok($"Replying post {newTweet} for user {newTweet.UserName}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Getting Exception from TweetsController.ReplyTweet(), while replying tweet.", ex.Message);
                return StatusCode(500, "Server Error while while replying tweet.");
            }
        }
    }
}
