using System.Threading.Tasks;

namespace WebAPIExecutor.APIClient
{
    public interface IWebAPIExecutor
    {
        Task InvokeDeleteAsync<T>(string uri);
        Task<T> InvokeGetAsync<T>(string uri);
        Task<T> InvokePostAsync<T>(string uri, T obj);
        Task InvokePutAsync<T>(string uri, T obj);
    }
}