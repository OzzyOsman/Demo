using BennettsBiking.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BennettsBiking.Controllers
{

    /// <summary>
    /// In order to keep the application simple aI have coded both an MVC and API controller and call the API from here
    /// rather than create another API application. Usually the controller code would be dumb and main procesing would be handled in 
    /// other classes but I wanted to get as much done as possible in a short time frame.
    /// 
    /// Also, please free to refer to the BGL Demo demo projects I prepared where I impleneted DI and Unit tests etc.
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "User Registration";

            return View(new UserModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:58376/api/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<UserModel>("Account", model);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        UserModel responseMmodel = postTask.Result.Content.ReadAsAsync<UserModel>().Result;

                        if (responseMmodel != null)
                        {
                            model = responseMmodel;
                        }

                        return View("Index", model);
                    }else
                    {
                        ModelState.AddModelError("", string.Format("{0} - Could not Create User Record", result.ReasonPhrase));
                    }
                }
            }

            return View("Index", model);
        }

    }
}
