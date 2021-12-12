using Microsoft.AspNetCore.Http;
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
        public IActionResult CreateTweet(string username, [FromBody] Tweet tweet)
        {
            return Ok($"Creating post {tweet} for user {username}" );
        }

        [HttpPut]
        [Route("/api/v1.0/tweets/{username}/update/{id}")]
        public IActionResult UpdateTweet(string username,int id)
        {
            return Ok($"Updating post with tweet id {id} for user {username}");
        }

        [HttpDelete]
        [Route("/api/v1.0/tweets/{username}/delete/{id}")]
        public IActionResult DeleteTweet(string username, int id)
        {
            return Ok($"Deleting post with tweet id {id} for user {username}");
        }

        [HttpPut]
        [Route("/api/v1.0/tweets/{username}/like/{id}")]
        public IActionResult LikeTweet(string username, int id)
        {
            return Ok($"Like post with tweet id {id} for user {username}");
        }

        [HttpPost]
        [Route("/api/v1.0/tweets/{username}/reply/{id}")]
        public IActionResult ReplyTweet(string username, [FromBody] Tweet tweet)
        {
            return Ok($"Replying post {tweet} for user {username}");
        }
    }
}
