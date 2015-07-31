using GroupBrush.BL.Users;
using GroupBrush.Entity;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace GroupBrush.Web.Public.Controllers
{
    public class RegisterController : ApiController
    {
        IUserService _userService;
        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }
        [Route("public/api/user")]
        [HttpPost]
        public HttpResponseMessage CreateUser(User user)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            if (user != null)
            {
                int? userId = null;
                userId = _userService.CreateAccount(user.UserName, user.Password);
                if (userId.HasValue && userId.Value > -1)
                {
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Name, userId.ToString()));
                    OwinHttpRequestMessageExtensions.GetOwinContext(this.Request).Authentication.
                    SignIn(identity);
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    response.Content = new StringContent("Success");
                }
            }
            return response;
        }
    }
}
