using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using GitHub.ApiServices.Interfaces;

namespace GitHub.ApiServices
{
    /// <summary>
    /// This class an it's Interface have been created solely for the purpose of putting a layer between
    /// the GithubApi class and the HttpClient so the HttpReponse can be Mocked and the GitHubApi class 
    /// can be unit tested
    /// </summary>
    public class HttpClientOperations : IHttpClientOperations
    {
        private HttpClient _httpClient;
        
        public HttpResponseMessage CallApi(string baseAddress, string apiMethod)
        {
            try
            {
                _httpClient = new HttpClient();

                _httpClient.BaseAddress = new Uri(baseAddress);
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "my-user-agent-name");

                return _httpClient.GetAsync(apiMethod).Result;
            }
            catch (AggregateException ex)
            {
                 throw ThrowException(apiMethod);
            }
        }

        private HttpException ThrowException(string apiName)
        {
            return new HttpException(string.Format("{0} is not available at the moment", apiName));
        }
    }
}