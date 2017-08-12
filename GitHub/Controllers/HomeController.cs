using System.Web.Mvc;
using GitHub.Dto;
using GitHub.Models;
using IGitHubApi = GitHub.ApiServices.Interfaces.IGitHubApi;

namespace GitHub.Controllers
{
    /// <summary>
    /// I had considered using Angular Js 1.? for the front end but decided that 
    /// that is quite old now and wasn't justifiable in the timescale for this demo 
    /// but decided it would be better to demonstrate unit testing, Mocking IOC etc
    /// </summary>
    public class HomeController : Controller
    {
        public IGitHubApi _gitHubApi;
       
        public HomeController(IGitHubApi gitHubApi)
        {
            _gitHubApi = gitHubApi;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult Search(UserSearchViewModel model)
        {            
            if (ModelState.IsValid)
            {
                GitHubUserModel userData = _gitHubApi.GetUserAndRepos(model.UserName).Result;

                if (userData != null)
                {
                    return View("SearchReults", userData);
                }
                else
                {
                    model.NoResults = true;
                    return View("Index", model);
                }
            }

            return View("Index");            
        }
    }
}