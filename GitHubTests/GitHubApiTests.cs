using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using GitHub.ApiServices;
using GitHub.ApiServices.Interfaces;
using GitHub.Dto;
using GitHub.Helpers;
using GitHub.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitHubTests
{
    [TestClass]
    public class GitHubApiTests
    {
        private IGitHubApi service { get; set; }
        private Mock<IHttpClientOperations> httpClientOperations { get; set; }

        [TestInitialize]
        public void Setup()
        {
            httpClientOperations = new Mock<IHttpClientOperations>();

            service = new GitHubApi(httpClientOperations.Object);
        }
        
        [TestMethod]
        public void GetUserAndReposTest_ValidResponse_WithMockHIHttpClientOperationsClass()
        {
            string userName = "username";
            string userPath = string.Format("users/{0}", userName);
            string repoPath = string.Format("users/{0}/repos", userName);
            UserSearchViewModel model = new UserSearchViewModel { UserName = userName };

            List<GitHubUserRepoModel> repoModels = new List<GitHubUserRepoModel> { 
                new GitHubUserRepoModel { Id = 1, Stargazers_Count = 1, Name = "Repo 1", Html_Url = "Repo 1 Url" },
                new GitHubUserRepoModel { Id = 2, Stargazers_Count = 2, Name = "Repo 2", Html_Url = "Repo 2 Url" },
                new GitHubUserRepoModel { Id = 3, Stargazers_Count = 3, Name = "Repo 3", Html_Url = "Repo 3 Url" },
                new GitHubUserRepoModel { Id = 2, Stargazers_Count = 4, Name = "Repo 4", Html_Url = "Repo 4 Url" },
                new GitHubUserRepoModel { Id = 5, Stargazers_Count = 5, Name = "Repo 5", Html_Url = "Repo 5 Url" }, 
                new GitHubUserRepoModel { Id = 6, Stargazers_Count = 6, Name = "Repo 6", Html_Url = "Repo 6 Url" }
            };
            
            GitHubUserModel userModel = new GitHubUserModel
            {
                Id = 1,
                Location = "Location",
                Login = userName,
                Avatar_Url = "url",
                Name = "Name",
                Repos = new List<GitHubUserRepoModel>()
            };


            var fakeUserResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<GitHubUserModel>(userModel,new JsonMediaTypeFormatter())
            };

            httpClientOperations.Setup(x => x.CallApi(Constants.GitHubEndpoint, userPath)).Returns(fakeUserResponse);


            var fakeRepoResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<List<GitHubUserRepoModel>>( repoModels,  new JsonMediaTypeFormatter())
            };

            httpClientOperations.Setup(x => x.CallApi(Constants.GitHubEndpoint, repoPath)).Returns(fakeRepoResponse);


            GitHubUserModel result = service.GetUserAndRepos(userName).Result;

            //Check User Data
            Assert.AreEqual(userModel.Id, result.Id);
            Assert.AreEqual(userModel.Location, result.Location);
            Assert.AreEqual(userModel.Login, result.Login);
            Assert.AreEqual(userModel.Avatar_Url, result.Avatar_Url);
            Assert.AreEqual(userModel.Repos, result.Repos);

            //Check Repos Counts, Sort Order Etc
            CollectionAssert.AreEqual(repoModels.OrderByDescending(x => x.Stargazers_Count).Take(5).ToList(), result.Repos);
            Assert.AreEqual(5, result.Repos.Count);
        }

        [TestMethod]
        public void GetUserAndReposTest_InvalidResponse_WithMockHIHttpClientOperationsClass()
        {
            string userName = "username";
            string userPath = string.Format("users/{0}", userName);
            string repoPath = string.Format("users/{0}/repos", userName);
            UserSearchViewModel model = new UserSearchViewModel { UserName = userName };

            List<GitHubUserRepoModel> repoModels = new List<GitHubUserRepoModel> {};

            GitHubUserModel userModel = new GitHubUserModel
            {
                Id = 1,
                Location = "Location",
                Login = userName,
                Avatar_Url = "url",
                Name = "Name",
                Repos = new List<GitHubUserRepoModel>()
            };


            var fakeUserResponse = new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                Content = new ObjectContent<GitHubUserModel>(userModel, new JsonMediaTypeFormatter())
            };

            httpClientOperations.Setup(x => x.CallApi(Constants.GitHubEndpoint, userPath)).Returns(fakeUserResponse);

            
            GitHubUserModel result = service.GetUserAndRepos(userName).Result;

            //Check User Data
            Assert.IsNull(result);
        }

        //Todo: Other Test scenarios
    }
}
