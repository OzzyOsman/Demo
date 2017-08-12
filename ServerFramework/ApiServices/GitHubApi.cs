using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ServerFramework.ApiServices.Interfaces;
using ServerFramework.Dto;
using ServerFramework.Helpers;

namespace ServerFramework.ApiServices
{
    public class GitHubApi : IGitHubApi
    {
        private string apiUrl = string.Empty;
      
        public GitHubApi()
        {
            apiUrl = ConfigManager.GetAppSetting(Constants.GitHubEndpoint);
        }

        //Todo: change return model to model with repos result and response result
        public async Task<GitHubUserModel> GetUserAndRepos(string userName)
        {
            string url = string.Format("users/{0}/repos", userName);
            GitHubUserModel userData = null;

            try
            {
                userData = await FindUser(userName);

                if (userData != null)
                {
                    List<GitHubUserRepoModel> repos = await FindRepos(url);

                    if (repos.Any())
                    {
                        repos = repos
                            .OrderByDescending(x => x.Stargazers_Count)
                            .Take(5)
                            .ToList();

                        userData.Repos = repos;
                    }
                }               
            }
            catch (Exception ex)
            {
                
            }

            return userData;
        }

        private async Task<GitHubUserModel> FindUser(string userName)
        {
            string url = string.Format("users/{0}", userName);

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "my-user-agent-name");

            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                GitHubUserModel result = await response.Content.ReadAsAsync<GitHubUserModel>();

                if (result != null && result.Id != null)
                {
                    return result;        
                }
            }

            return null;
        }

        private async Task<List<GitHubUserRepoModel>> FindRepos(string reposPath)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "my-user-agent-name");

            var response = client.GetAsync(reposPath).Result;

            if (response.IsSuccessStatusCode)
            {
                List<GitHubUserRepoModel> result = await response.Content.ReadAsAsync<List<GitHubUserRepoModel>>();

                return result;
            }

            return new List<GitHubUserRepoModel>();
        }
    }
}