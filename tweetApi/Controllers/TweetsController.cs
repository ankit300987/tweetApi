using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace tweetApi.Controllers
{
    [ApiController]
    [Route("api/v1.0/[controller]")]
    public class TweetsController : ControllerBase
    {
        private readonly ILogger<TweetsController> logger;
        private IEnumerable<Tweets> tweets { get; set; }
        public TweetsController(ILogger<TweetsController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult All()
        {
            logger.LogInformation("Getting All tweets");
            return Ok("Receiving all tweets");
        }

        [HttpGet("{username}")]
        public IActionResult GetTweetsOfUserName(string username)
        {
            logger.LogInformation("Receiving all tweets of the user " + username);
            return Ok("Receiving all tweets of the user " + username);
        }

        [HttpPost]
        [Route("/api/v1.0/tweets/{username}/add")]
        public IActionResult CreateTweet([FromBody] Tweet tweet)
        {
            return Ok($"Creating post {tweet} for user {tweet.UserName}");
        }

        [HttpPut]
        [Route("/api/v1.0/tweets/{username}/update/{id}")]
        public IActionResult UpdateTweet(Tweet tweet)
        {
            return Ok($"Updating post with tweet id {tweet.Id} for user {tweet.UserName}");
        }

        [HttpDelete]
        [Route("/api/v1.0/tweets/{username}/delete/{id}")]
        public IActionResult DeleteTweet(Tweet tweet)
        {
            return Ok($"Deleting post with tweet id {tweet.Id} for user {tweet.UserName}");
        }

        [HttpPut]
        [Route("/api/v1.0/tweets/{username}/like/{id}")]
        public IActionResult LikeTweet(Tweet tweet)
        {
            return Ok($"Like post with tweet id {tweet.Id} for user {tweet.UserName}");
        }

        [HttpPost]
        [Route("/api/v1.0/tweets/{username}/reply/{id}")]
        public IActionResult ReplyTweet([FromBody] Tweet tweet)
        {
            return Ok($"Replying post {tweet} for user {tweet.UserName}");
        }
    }
}
