using System.Net;
using System.Threading;
using System.Web.Http;
using AppClinic.Models;
using AppClinic.Security;
using AppClinic.BusinessLogic;

namespace AppClinic.Controllers
{
    /// <summary>
    /// login controller class for authenticate users
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {

        IBusinessUser businesUser;
        public LoginController()
        {
            businesUser = new BusinessUser();
        }

        public LoginController(IBusinessUser businesUserParam)
        {
            businesUser = businesUserParam;
        }


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

            User user = businesUser.GetUserByCredentials(login);
            if (user != null)
            {
                var rolename = user.Role;
                var token = TokenGenerator.GenerateTokenJwt(user.Username, user.Role);
                return Ok(token);
            }

            // Unauthorized access 
            return Unauthorized();
        }

        
    }
}
