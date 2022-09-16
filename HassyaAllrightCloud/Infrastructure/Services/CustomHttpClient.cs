using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace HassyaAllrightCloud.Infrastructure.Services
{
    public interface ICustomeHttpClient
    {
        Task AddRequestHeaders(HttpClient httpClient);
        Task<T> GetJsonAsync<T>(string requestUri);
        Task<HttpResponseMessage> PostJsonAsync<T>(string requestUri, T content);
        Task<HttpResponseMessage> PutJsonAsync<T>(string requestUri, T content);
        StringContent getStringContentFromObject(object obj);
        Task<HttpClient> GetHttpClient();
    }

    public class CustomHttpClient : HttpClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public CustomHttpClient(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        public async Task AddRequestHeaders(HttpClient httpClient)
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            httpClient.DefaultRequestHeaders.Remove("Cookie");
            var cookies = _httpContextAccessor.HttpContext.Request.Cookies.Keys;
            foreach (var key in cookies)
            {

                httpClient.DefaultRequestHeaders.Add("Cookie", $"{key}={_httpContextAccessor.HttpContext.Request.Cookies[key]}");
            }
            
        }

        public async Task<T> GetJsonAsync<T>(string requestUri)
        {
            HttpClient httpClient = _httpClient;
            var httpContent = await httpClient.GetAsync(requestUri);
            string jsonContent = httpContent.Content.ReadAsStringAsync().Result;
            T obj = JsonConvert.DeserializeObject<T>(jsonContent);
            httpContent.Dispose();
            return obj;
        }
        public async Task<HttpResponseMessage> PostJsonAsync<T>(string requestUri, T content)
        {
            HttpClient httpClient = _httpClient;
            string myContent = JsonConvert.SerializeObject(content);
            StringContent stringContent = new StringContent(myContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(requestUri, stringContent);
            return response;
        }
        public async Task<HttpResponseMessage> PutJsonAsync<T>(string requestUri, T content)
        {
            HttpClient httpClient = _httpClient;
            string myContent = JsonConvert.SerializeObject(content);
            StringContent stringContent = new StringContent(myContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(requestUri, stringContent);
            return response;
        }
        public StringContent getStringContentFromObject(object obj)
        {
            var serialized = JsonConvert.SerializeObject(obj);
            var stringContent = new StringContent(serialized, Encoding.UTF8, "application/json");
            return stringContent;
        }

        public async Task<HttpClient> GetHttpClient()
        {
            HttpClient httpClient = _httpClient;
            await AddRequestHeaders(httpClient);
            return httpClient;
        }

        public new Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return _httpClient.GetAsync(requestUri);
        }

        public new Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return _httpClient.PostAsync(requestUri, content);
        }

        public new Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
        {
            return _httpClient.PutAsync(requestUri, content);
        }
    }
}