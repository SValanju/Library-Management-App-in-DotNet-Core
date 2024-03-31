using Library_WebApp.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Library_WebApp.Services.ConsumeAPI
{
    public class ConsumeAPI : IConsumeAPI
    {
        public async Task<string> PostAsync(string endPoint, string requestBody, string token)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                //httpClient.BaseAddress = new Uri(endPoint);
                httpClient.DefaultRequestHeaders.Clear();

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrWhiteSpace(token))
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage httpResponse = await httpClient.PostAsync(endPoint, new StringContent(requestBody, Encoding.UTF8, "application/json"));

                var response = await httpResponse.Content.ReadAsStringAsync();

                return !string.IsNullOrWhiteSpace(response) ? response : $"{{'status': {(int)httpResponse.StatusCode}, 'message': '{httpResponse.ReasonPhrase}'}}";
            }
        }

        public async Task<string> GetAsync(string endPoint, string token)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                //httpClient.BaseAddress = new Uri(endPoint);
                httpClient.DefaultRequestHeaders.Clear();

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrWhiteSpace(token))
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage httpResponse = await httpClient.GetAsync(endPoint);

                var response = await httpResponse.Content.ReadAsStringAsync();

                return !string.IsNullOrWhiteSpace(response) ? response : $"{{'status': {(int)httpResponse.StatusCode}, 'message': '{httpResponse.ReasonPhrase}'}}";
            }
        }

        public async Task<string> PutAsync(string endPoint, string requestBody, string token)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                //httpClient.BaseAddress = new Uri(endPoint);
                httpClient.DefaultRequestHeaders.Clear();

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrWhiteSpace(token))
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage httpResponse = await httpClient.PutAsync(endPoint, new StringContent(requestBody, Encoding.UTF8, "application/json"));

                var response = await httpResponse.Content.ReadAsStringAsync();

                return !string.IsNullOrWhiteSpace(response) ? response : $"{{'status': {(int)httpResponse.StatusCode}, 'message': '{httpResponse.ReasonPhrase}'}}";
            }
        }
    }
}
