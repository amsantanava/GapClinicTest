using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http;
using AppClinic.Models;
using AppClinic.Security;

namespace AppClinic.Controllers
{
    /// <summary>
    /// login controller class for authenticate users
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        private ClinicDbContext context = new ClinicDbContext();

        
        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            var isInRole = Thread.CurrentPrincipal.IsInRole("Admin");
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated} - Es Admin: {isInRole}");
        }

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            User user = GetUserByCredentials(login);
            if (user != null)
            {
                var rolename = user.Role;
                var token = TokenGenerator.GenerateTokenJwt(user.Username, user.Role);
                return Ok(token);
            }

            // Unauthorized access 
            return Unauthorized();
        }

        //This method find the user with the credentials and if exists return the user 
        private User GetUserByCredentials(LoginRequest login)
        {
            return context.Users.Where(
                u => u.Username == login.Username 
                && u.Password == login.Password
                ).FirstOrDefault();
        }
    }
}
