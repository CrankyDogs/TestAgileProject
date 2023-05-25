using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Net;
using System.Security.Claims;
using TestProjLarge.Entities;
using TestProjLarge.Infrastructures;

namespace TestProjLarge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    
    public class CommonController : ControllerBase
    {
        [HttpGet]
        [Route("CommonGet")]

        public IActionResult Get(string path)
        {
            NavApiConfig config = ConfigJSON.Read();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            //var claimsIdentity = identity as ClaimsIdentity;
            
                
            if (identity != null)
            {
                var usernameClaim = identity.FindFirst("Username");
                var passwordClaim = identity.FindFirst("Password");

              if (usernameClaim != null && passwordClaim != null)
                {
                    config.NavUserName = usernameClaim.Value;
                    config.NavPassword = passwordClaim.Value;
                }
                else
                {
                    // Handle the case where the required claims are not found
                    // You can set default values or take appropriate action
                    config.NavUserName = "SR0002";
                    config.NavPassword = "Admin@2022";
                }
            }
            else
            {
                config.NavUserName = "";
                config.NavPassword = "";
            }
            string url;
            if (path == "GetCompanyId")
            {
                url = config.NavApiBaseUrl + config.NavPath + $"Company({config.NavCompanyId})";
            }
            else if (path == "GetCompanyIdReport")
            {
                url = config.NavApiBaseUrl + config.NavPath + $"Company(''Autoparts International Pvt.L'')";
            }
            else
            {
                url = config.NavApiBaseUrl + config.NavPath + $"Company({config.NavCompanyId})/" + path;
            }

            var client = Nav.NavClient(url, config);

            var request = new RestRequest(Method.GET);

            IRestResponse<dynamic> response = client.Execute<dynamic>(request);
            if (response.StatusCode == HttpStatusCode.OK)
                return Ok(response.Content);
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized(new { code = "Unauthorized", message = "Session Timed Out. Login Again." });
            else
                return BadRequest(response.Content);
        }
    }
}
