using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using GitHub.ApiServices.Interfaces;
using Constants = GitHub.Helpers.Constants;
using GitHubUserModel = GitHub.Dto.GitHubUserModel;
using GitHubUserRepoModel = GitHub.Dto.GitHubUserRepoModel;
using IGitHubApi = GitHub.ApiServices.Interfaces.IGitHubApi;

namespace GitHub.ApiServices
{
    /// <summary>
    /// The Repo Data is quite big and the fields to be displayed from the repos was not specified in the Requirements.
    ///
    /// In fairness I could have requested more information regarding the data to be displayed but I figured
    /// that displaying sufficient information to demonstrate that the TOP 5 were being correctly sorted on the 
    /// StarGazer_Count would suffice.
    /// </summary>
    public class GitHubApi : IGitHubApi
    {
        private string _apiBaseUrl = string.Empty;
        private IHttpClientOperations _httpClientOperations;

        public GitHubApi(IHttpClientOperations iHttpClientOperations)
        {
            _httpClientOperations = iHttpClientOperations;
            _apiBaseUrl = Constants.GitHubEndpoint;
        }

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
                Elmah.ErrorLog
                    .GetDefault(HttpContext.Current)
                    .Log(new Elmah.Error(ex));
            }

            return userData;
        }


        private async Task<GitHubUserModel> FindUser(string userName)
        {
            string url = string.Format("users/{0}", userName);

            var response = _httpClientOperations.CallApi(_apiBaseUrl, url);

            if (response.IsSuccessStatusCode)
            {
                GitHubUserModel result = await response.Content.ReadAsAsync<GitHubUserModel>();

                if (result != null && result.Id != null && !string.IsNullOrEmpty(result.Location) && !string.IsNullOrEmpty(result.Name))
                {
                    return result;        
                }
            }

            return null;
        }

        private async Task<List<GitHubUserRepoModel>> FindRepos(string repoPath)
        {
            var response = _httpClientOperations.CallApi(_apiBaseUrl, repoPath);

            if (response.IsSuccessStatusCode)
            {
                List<GitHubUserRepoModel> result = await response.Content.ReadAsAsync<List<GitHubUserRepoModel>>();

                return result;
            }

            return new List<GitHubUserRepoModel>();
        }
    }
}