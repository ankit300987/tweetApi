using Core.Models;
using DataSource.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace tweetApi.Controllers
{
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<UsersController> Logger;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> Logger)
        {
            this.userRepository = userRepository;
            this.Logger = Logger;
        }
        [HttpGet]
        [Route("/api/v1.0/tweets/users/all")]
        public IActionResult GetUsers()
        {
            try
            {

                var users = userRepository.GetAllUsers();
                Logger.LogInformation($"Getting all the users from the repository");
                return Ok(users);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occured at UserController.GetUsers(). Exception message is {ex.Message}");
                return StatusCode(500, "Error on the server while fetching all users data");
            }
        }

        [HttpPost]
        [Route("/api/v1.0/tweets/register")]
        public IActionResult RegisterUser([FromBody] User user)
        {
            try
            {
                string msg = userRepository.RegisterUser(user);
                if (string.IsNullOrEmpty(msg))
                    return Ok($"User with username {user.UserName} created");
                return BadRequest(msg);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occured at UserController.RegisterUser(). Exception message is {ex.Message} ");
                return StatusCode(500, "Error on the server while registering a user");
            }
        }

        [HttpPost]
        [Route("/api/v1.0/tweets/login")]
        public IActionResult Login([FromBody] Credential userdata)
        {
            return Ok($"Login with the user emailid {userdata.EmailId} ");
        }

        [HttpGet]
        [Route("/api/v1.0/tweets/users/search/{username}/")]
        public IActionResult SearchUser(string username)
        {
            try
            {
                User user = userRepository.SearchUser(username);
                return user != null ? Ok(user) : NotFound($"User with username {username} could not be found");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occured at UserController.SearchUser(). Exception message is {ex.Message} ");
                return StatusCode(500, "Error on the server while searching a user");
            }
        }

        [HttpGet]
        [Route("/api/v1.0/tweets/{username}/forgot")]
        public IActionResult ForgotPassword(string username)
        {
            return Ok($"Resetting password for the user {username}");
        }

        [HttpGet]
        [Route("/api/v1.0/tweets/users/search/{id}/")]
        public IActionResult SearchUserById(int id)
        {
            try
            {
                User user = userRepository.GetUserById(id);
                return user != null ? Ok(user) : NotFound($"User with id {id} could not be found");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occured at UserController.SearchUserById(). Exception message is {ex.Message} ");
                return StatusCode(500, "Error on the server while searching a user");
            }
        }
    }
}
