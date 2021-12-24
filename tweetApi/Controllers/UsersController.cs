using Core.Models;
using DataSource.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using tweetApi.Auth;
using tweetApi.Filters;

namespace tweetApi.Controllers
{
    [ApiController]
    [TokenAuthFilter]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<UsersController> Logger;
        private readonly ICustomTokenManager customTokenManager;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> Logger, ICustomTokenManager customTokenManager)
        {
            this.userRepository = userRepository;
            this.Logger = Logger;
            this.customTokenManager = customTokenManager;
        }
        [HttpGet]
        [Route("/api/v1.0/tweets/users/all")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {

                var users = await userRepository.GetAllUsersAsyc();
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
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            try
            {
                string msg = await userRepository.RegisterUserAsync(user);
                if (string.IsNullOrEmpty(msg))
                    return Ok(user);
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
        public async Task<IActionResult> LoginAsync([FromBody] Credential userdata)
        {
            try
            {
                User user = await userRepository.LoginUserAsync(userdata.UserName, userdata.Password);
                if (user == null) return NotFound($"User with {userdata.UserName} was not found");
                var token = await customTokenManager.CreateTokenAsync(user.UserName);
                return await Task.FromResult(Ok(token));
            }
            catch (Exception)
            {

                return StatusCode(500, "Error on the server while logining in a user"); 
            }
        }

        [HttpGet]
        [Route("/api/v1.0/tweets/users/search/{username}/")]
        public async Task<IActionResult> SearchUserAsync(string username)
        {
            try
            {
                User user = await userRepository.SearchUserAsync(username);
                return user != null ? Ok(user) : NotFound($"User with username {username} could not be found");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occured at UserController.SearchUser(). Exception message is {ex.Message} ");
                return StatusCode(500, "Error on the server while searching a user");
            }
        }

        [HttpPut]
        [Route("/api/v1.0/tweets/{username}/forgot")]
        public async Task<IActionResult> ForgotPasswordAsync(string username, [FromBody] User updatedUser)
        {
            User user = await userRepository.SearchUserAsync(username);
            if(user == null)
            {
                return NotFound($"User with username {username} could not be found");
            }
            if (user.EmailId != updatedUser.EmailId) return BadRequest("Email id of the provided user is not matching");
            user.Password = updatedUser.Password;
            user.ConfirmPassword = updatedUser.ConfirmPassword;
            await userRepository.UpdateUserAsync(user);
            return await Task.FromResult(Ok($"Resetting password for the user {username}"));
        }

        [HttpGet]
        [Route("/api/v1.0/tweets/users/searchbyid/{id}/")]
        public async Task<IActionResult> SearchUserByIdAsync(int id)
        {
            try
            {
                User user = await userRepository.GetUserByIdAsync(id);
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
