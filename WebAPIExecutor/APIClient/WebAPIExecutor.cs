using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIExecutor.APIClient
{
    public class WebAPIExecutor : IWebAPIExecutor
    {
        private readonly string baseUrl;
        private readonly HttpClient httpClient;

        public WebAPIExecutor(string baseUrl, HttpClient httpClient)
        {
            this.baseUrl = baseUrl;
            this.httpClient = httpClient;

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> InvokeGetAsync<T>(string uri)
        {
            return await httpClient.GetFromJsonAsync<T>(GetUrl(uri));
        }

        public async Task<T> InvokePostAsync<T>(string uri, T obj)
        {
            var response = await httpClient.PostAsJsonAsync(GetUrl(uri), obj);
            await HandleError(response);

            return await response.Content.ReadFromJsonAsync<T>();
        }


        public async Task InvokePutAsync<T>(string uri, T obj)
        {
            var response = await httpClient.PutAsJsonAsync(GetUrl(uri), obj);
            await HandleError(response);
        }

        public async Task InvokeDeleteAsync<T>(string uri)
        {
            var response = await httpClient.DeleteAsync(GetUrl(uri));
            await HandleError(response);
        }
        private static async Task HandleError(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(message);
            }
        }

        private string GetUrl(string uri)
        {
            return $"{baseUrl}/{uri}";
        }
    }
}
