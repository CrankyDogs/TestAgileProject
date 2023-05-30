using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Net;
using System.Security.Claims;
using TestProjLarge.Entities;
using TestProjLarge.Infrastructures;

namespace TestProjLarge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

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
                config.NavUserName = identity.FindFirst("Username").Value;
                config.NavPassword = identity.FindFirst("Password").Value;
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

        [Route("CommonPost")]
        [HttpPost]

        public IActionResult CommonPost(string path, [FromBody] dynamic data)
        {
            var stringData = Convert.ToString(data);
            NavApiConfig config = ConfigJSON.Read();
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                config.NavUserName = identity.FindFirst("Username").Value;
                config.NavPassword = identity.FindFirst("Password").Value;
            }
            else
            {
                config.NavUserName = "";
                config.NavPassword = "";
            }

            string url = config.NavApiBaseUrl + config.NavPath + $"Company({config.NavCompanyId})/" + path;
            var client = Nav.NavClient(url, config);
            var request = new RestRequest();
            request.AddJsonBody(stringData);

            IRestResponse<dynamic> response = client.Post<dynamic>(request);

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                return Ok(response.Data);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized) {
                return Unauthorized(new { code = "Unauthorized", message = "Session Timed Out, Login Again"
                });
            }
            else
            {
                return BadRequest(response.Content);
            }
        }

        [Route("CallFunction")]
        [HttpPost]

        public IActionResult CallFunction(string path, string companyId, [FromBody] dynamic data)
        {
            var stringData = Convert.ToString(data);
            NavApiConfig config = ConfigJSON.Read();
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                config.NavUserName = identity.FindFirst("Username").Value;
                config.NavPassword = identity.FindFirst("Password").Value;

            }
            else
            {
                config.NavUserName = "";
                config.NavPassword = "";
            }

            string url = config.NavApiBaseUrl + config.NavPath + path + $"?Company=" + companyId;
            var client = Nav.NavClient(url, config);

            var request = new RestRequest();
            request.AddJsonBody(stringData);
            IRestResponse<dynamic> response = client.Post<dynamic>(request);
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.NoContent) {
                return Ok(response.Data);
            } else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized(new { code = "Unauthorized", message = "Session Timed Out, Login Again" });

            }
            else
            {
                return BadRequest(response.Content);
            }

        }


        [Route("CallReport")]
        [HttpPost]

        public IActionResult CallReport(string path, string companyId, [FromBody] dynamic data) {
            var stringData = Convert.ToString(data);
            NavApiConfig config = ConfigJSON.Read();
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                config.NavUserName = identity.FindFirst("Username").Value;
                config.NavPassword = identity.FindFirst("Password").Value;

            }
            else
            {
                config.NavUserName = "";
                config.NavPassword = "";
            }

            string url = config.NavApiBaseUrl + config.NavPath + path + $"?Company=" + companyId;
            var client = Nav.NavClient(url, config);

            var request = new RestRequest();
            request.AddJsonBody(stringData);

            IRestResponse<dynamic> response = client.Post<dynamic>(request);

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.NoContent)
            {
                return Ok(response.Data);
            } else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized(new { code = "Unauthorized", message = "Session Timed Out, Login Again" });
            }
            else
            {
                return BadRequest(response.Content);
            }
        }

        [Route("CheckPassword")]
        [HttpGet]

        public HttpStatusCode CheckPassword(string oldPswd)
        {
            NavApiConfig config = ConfigJSON.Read();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string pwd = config.NavPassword = identity.FindFirst("Password").Value;
            if (oldPswd == pwd)
            {
                return HttpStatusCode.OK;
            }
            else
            {
                return HttpStatusCode.Unauthorized;
            }
        }

        [Route("Patch")]
        [HttpPatch]

        public IActionResult Patch(string path, [FromBody] dynamic data)
        {
            var stringData = Convert.ToString(data);
            NavApiConfig config = ConfigJSON.Read();
            var identity = HttpContext.User.Identities as ClaimsIdentity;

            if (identity != null)
            {
                config.NavUserName = identity.FindFirst("Username").Value;
                config.NavPassword = identity.FindFirst("Password").Value;
            }
            else
            {
                config.NavUserName = "";
                config.NavPassword = "";
            }

            string url = config.NavApiBaseUrl + config.NavPath + $"Company({config.NavCompanyId})/" + path;
            var client = Nav.NavClient(url, config);

            var request = new RestRequest();
            request.AddHeader("If-Match", "*");
            request.AddJsonBody(stringData);
            IRestResponse<dynamic> response = client.Patch<dynamic>(request);

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                return Ok(response.Data);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized(new { code = "Unauthorized", message = "Session Timed Out, Login Again" });

            }
            else
            {
                return BadRequest(response.Content);
            }

        }

        [Route("Delete")]
        [HttpDelete]

        public IActionResult Delete(string path)
        {
            NavApiConfig config = ConfigJSON.Read();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                config.NavUserName = identity.FindFirst("Username").Value;
                config.NavPassword = identity.FindFirst("Password").Value;
            }
            else
            {
                config.NavUserName = "";
                config.NavPassword = "";
            }

            string url = config.NavApiBaseUrl + config.NavPath + $"Company({config.NavCompanyId})/" + path;

            var client = Nav.NavClient(url, config);

            var request = new RestRequest(Method.DELETE);
            IRestResponse<dynamic> response = client.Execute<dynamic>(request);

            if(response.IsSuccessful && response.StatusCode == HttpStatusCode.NoContent)
            {
                return Ok(response.Data);
            }else if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized(new { code = "Unauthorized", message = "Session Timed Out, Login Again" });
            }
            else
            {
                return BadRequest(response.Content);
            }
        }
    }
}
