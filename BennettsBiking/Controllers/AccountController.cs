using BennettsBiking.DAL;
using BennettsBiking.Models;
using BennettsBiking.DomainModels;
using System.Web.Http;
using System;

namespace BennettsBiking.Controllers
{
    /// <summary>
    /// In order to keep the application simple I have not moved the logic(context createtion etc) outside of the controller method.
    /// </summary>
    public class AccountController : ApiController
    {
        [HttpPost]
        public IHttpActionResult PostNewUser(UserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            try
            {
                using (BennetsContext ctx = new BennetsContext())
                {
                    User newUser = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DateOfBirth = DateTime.Parse(model.DateOfBirth)
                    };

                    ctx.User.Add(newUser);                                     
                    ctx.SaveChanges();

                    model.Id = newUser.Id;
                    model.FirstName = newUser.FirstName;
                    model.LastName = newUser.LastName;
                    model.DateOfBirth = newUser.DateOfBirth.ToString(" dd MMM yyyy");

                    return Ok(model);
                }
            }
            catch(Exception ex)
            {
                return BadRequest("Could not create user record");
            }
        }
    }
}
