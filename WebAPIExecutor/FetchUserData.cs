using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIExecutor.APIClient;

namespace WebAPIExecutor
{
    public class FetchUserData
    {
        private readonly IWebAPIExecutor webAPIExecutor;

        public FetchUserData(IWebAPIExecutor webAPIExecutor)
        {
            this.webAPIExecutor = webAPIExecutor;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await webAPIExecutor.InvokeGetAsync<IEnumerable<User>>("/api/v1.0/tweets/users/all");
        }

        public async Task<User> Register(User user)
        {
            return await webAPIExecutor.InvokePostAsync<User>("/api/v1.0/tweets/register", user);
        }

        /// <summary>
        /// Todo Implement user Login scenario
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        //"/api/v1.0/tweets/login" post

        public async Task<User> GetUsers(string username)
        {
            return await webAPIExecutor.InvokeGetAsync<User>($"/api/v1.0/tweets/users/search/{username}/");
        }

        public async Task UpdateUserAsync(User user, string username)
        {
            await webAPIExecutor.InvokePutAsync($"/api/v1.0/tweets/{username}/forgot ", user);
        }

        public async Task<User> GetUserById(int id)
        {
            return await webAPIExecutor.InvokeGetAsync<User>($"/api/v1.0/tweets/users/search/{id}/");
        }
    }
}
