using System;
using System.Web.Mvc;
using GitHub.ApiServices.Interfaces;
using GitHub.Controllers;
using GitHub.Dto;
using GitHub.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitHubTests
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController controller { get; set; }
        private Mock<IGitHubApi> gitHubApi { get; set; }

        [TestInitialize]
        public void Setup()
        {
            gitHubApi = new Mock<IGitHubApi>();

            controller = new HomeController(gitHubApi.Object);
        }
        
        [TestMethod]
        public void IndexActionViewTest()
        {
            ViewResult view =  controller.Index() as ViewResult;

            Assert.AreEqual("Index", view.ViewName);
        }
        
        [TestMethod]
        public void SeatchActionViewTest_CalledWithNullModelAndInvalidModelState()
        {
            controller.ModelState.AddModelError("Test", "TestState");

            ViewResult view = controller.Search(null) as ViewResult;

            Assert.AreEqual("Index", view.ViewName);
        }

        [TestMethod]
        public void SeatchActionViewTest_CalledWithNullModelAndValidModelState()
        {
            try
            {
                ViewResult view = controller.Search(null) as ViewResult;
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Object reference not set to an instance of an object.", ex.Message);
                Assert.AreEqual( typeof(NullReferenceException), ex.GetType());
            }
        }

        [TestMethod]
        public void SeatchActionViewTest_CalledWithValidModelAndValidModelState_WithMockGitHubApiClass()
        {
            string login = "username";
            UserSearchViewModel model = new UserSearchViewModel { UserName = login };

            GitHubUserModel expectedData = new GitHubUserModel
            {
                Id = 1,
                Location = "Location",
                Login = login,
                Avatar_Url = "url"
            };

            gitHubApi.Setup(x => x.GetUserAndRepos(login)).ReturnsAsync(expectedData);
            
            ViewResult view = controller.Search(model) as ViewResult;
            GitHubUserModel resultModel = view.ViewData.Model as GitHubUserModel;

            Assert.AreEqual("SearchReults", view.ViewName);
            Assert.AreEqual(expectedData.GetType(), view.ViewData.Model.GetType());

            Assert.IsNotNull(resultModel.Id);
            Assert.AreEqual(resultModel.Id, expectedData.Id);
            Assert.AreEqual(resultModel.Location, expectedData.Location);
            Assert.AreEqual(resultModel.Login, expectedData.Login);
            Assert.AreEqual(resultModel.Avatar_Url, expectedData.Avatar_Url);
        }

        [TestMethod]
        public void SeatchActionViewTest_CalledWithInValidModelAndValidModelState_WithMockGitHubApiClass()
        {
            string login = "username";
            UserSearchViewModel model = new UserSearchViewModel { UserName = login };
            GitHubUserModel searchModel = null;

            gitHubApi.Setup(x => x.GetUserAndRepos(It.IsAny<string>())).ReturnsAsync(searchModel);
            
            ViewResult view = controller.Search(model) as ViewResult;
            UserSearchViewModel resultModel = view.ViewData.Model as UserSearchViewModel;
            
            Assert.AreEqual("Index", view.ViewName);
            Assert.IsTrue(resultModel.NoResults);        
        }
    }
}
