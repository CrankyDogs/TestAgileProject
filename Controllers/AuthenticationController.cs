using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using TestProjLarge.Entities;
using TestProjLarge.Infrastructures;

namespace TestProjLarge.Controllers

{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppSettings _appSettings;

        public AuthenticationController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        [HttpPost]

        public IActionResult Authentication([FromBody] login data)
        {
            NavApiConfig config = ConfigJSON.Read();
            config.NavUserName = data.Username;
            config.NavPassword = data.Password;

            string url = config.NavApiBaseUrl + config.NavPath + $"Company({config.NavCompanyId})";
            var client = Nav.NavClient(url, config);

            var request = new RestRequest();
            IRestResponse<dynamic> response = client.Execute<dynamic>(request);

            if(response.StatusCode == HttpStatusCode.OK)
            {
                var user = new User();
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("Username", config.NavUserName),
                        new Claim("Password", data.Password)
                    }),

                    Expires = DateTime.UtcNow.AddYears(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.token = tokenHandler.WriteToken(token);
                user.Username = config.NavUserName;
                user.expiresAt = tokenDescriptor.Expires;
                return Ok(user);
            }
            else if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized(new { error = new { code = "Unauthorized", message = "Invalid Username or Password, Try Again" } });

            }
            else
            {
                return BadRequest(response.Content);
            }
        }

    }
}
