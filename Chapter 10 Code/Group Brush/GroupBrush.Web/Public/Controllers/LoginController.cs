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
    public class LoginController : ApiController
    {
        IUserService _userService;
        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        [Route("public/api/loginStatus")]
        [HttpGet]
        public string GetLoginStatus()
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                return "loggedIn";
            }
            else
            {
                return "loggedOut";
            }
        }
        [Route("public/api/login")]
        [HttpPost]
        public HttpResponseMessage Login(UserLogin login)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
            if (login != null)
            {
                int? userId;
                if (_userService.ValidateUserLogin(login.UserName, login.Password, out userId))
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
        [Route("public/api/logout")]
        [HttpPost]
        public string Logout()
        {
            OwinHttpRequestMessageExtensions.GetOwinContext(this.Request).Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return "Success";
        }
    }
}
