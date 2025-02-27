using CollegeApp.LoginModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LogginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LogginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        public ActionResult Login(LoginRequestDTO loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Pls provide username and password");
            }
            LoginResponseDTO loginResponse = new() { UserName = loginRequest.UserName };
            if (loginRequest.UserName == "Sudharshan" && loginRequest.Password == "123")
            {
                // Genarate JWT 
                // var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretForLocalUsers"));
                if (ReturnSecrentKey(loginRequest.PolicyName).IsNullOrEmpty())
                {
                    return Ok("Invalid Policy Name");
                }
                var key = ReturnSecrentKey(loginRequest.PolicyName);
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, loginRequest.UserName),
                        new Claim(ClaimTypes.Role,"Admin"),
                        new Claim(ClaimTypes.Role,"SuperAdmin")
                    }),
                    Expires = DateTime.Now.AddHours(4),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                loginResponse.Token = tokenHandler.WriteToken(token);
            }
            else
            {
                return Ok("Invalid UserName and Password");
            }
            return Ok(loginResponse);

            byte[] ReturnSecrentKey(string policyname)
            {
                switch (loginRequest.PolicyName)
                {
                    case "LoginForGoogleUser":
                        return Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretForGoogle"));
                    case "LoginForMicroSoftUser":
                        return Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretForMicroSoft"));
                    case "LoginForLocalUsers":
                        return Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretForLocalUsers"));
                }
                return null;
            }
        }
    }
}
