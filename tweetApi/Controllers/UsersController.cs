using Microsoft.AspNetCore.Mvc;
using tweetApi.Filters;

namespace tweetApi.Controllers
{
    [ApiController]
    public class UsersController : Controller
    {
        [HttpGet]
        [Route("/api/v1.0/tweets/users/all")]
        public IActionResult GetUsers()
        {
            return Ok("Receiving all the users");
        }

        [HttpPost]
        [Route("/api/v1.0/tweets/register")]
        [User_PasswordConfirmPasswordSame]
        public IActionResult RegisterUser([FromBody] User user)
        {
            return Ok($"Registering a new user with email id {user.EmailId}");
        }

        [HttpPost]
        [Route("/api/v1.0/tweets/login")]
        public IActionResult login([FromBody] Credential userdata)
        {
            return Ok($"Login with the user emailid {userdata.EmailId} ");
        }

        [HttpGet]
        [Route("/api/v/1.0/tweets/users/search/{username}/")]
        public IActionResult SearchUser(string username)
        {
            return Ok($"Searching for the user {username}");
        }

        [HttpGet]
        [Route("/api/v1.0/tweets/{username}/forgot")]
        public IActionResult forGotPassword(string username)
        {
            return Ok($"Resetting password for the user {username}");
        }
    }
}
