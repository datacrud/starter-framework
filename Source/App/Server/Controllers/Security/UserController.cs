using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Project.Server.Models;
using Project.Server.Service;

namespace Project.Server.Controllers.Security
{
    [Authorize(Roles = "SystemAdmin, Admin")]
    [RoutePrefix("api/User")]
    public class UserController : SecurityBaseController<ApplicationUser>
    {
        private readonly IUserService _service;

        public UserController(IUserService service) : base(service)
        {
            _service = service;
        }
        

        //User Section
        [HttpGet]
        [Route("GetUsers")]
        public IHttpActionResult GetUsers()
        {
            return Ok(_service.GetUsers());
        }

        [HttpGet]
        [Route("GetUser")]
        public IHttpActionResult GetUser(string id)
        {
            return Ok(_service.GetUser(id));
        }


        [HttpPost]
        [Route("CreateUser")]
        public IHttpActionResult CreateUser(RequestModels.UserCreateRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
        
            return Ok(_service.CreateUser(model));
        }        


        [HttpPut]
        [Route("UpdateUser")]
        public IHttpActionResult UpdateUser(RequestModels.UserCreateRequestModel model)
        {
            return Ok(_service.UpdateUser(model));
        }


        [HttpDelete]
        [Route("DeleteUser")]
        public IHttpActionResult DeleteUser(string id)
        {
            if (HttpContext.Current.User.Identity.GetUserId() == id)
                return BadRequest();

            return Ok(_service.DeleteUser(id));
        }
    }
}
