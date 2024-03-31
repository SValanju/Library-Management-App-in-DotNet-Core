using Library_WebApp.Models;
using Newtonsoft.Json.Linq;

namespace Library_WebApp.Services.ConsumeAPI
{
    public interface IConsumeAPI
    {
        Task<string> PostAsync(string endPoint, string requestBody, string token = "");

        Task<string> GetAsync(string endPoint, string token = "");

        Task<string> PutAsync(string endPoint, string requestBody, string token = "");
    }
}
