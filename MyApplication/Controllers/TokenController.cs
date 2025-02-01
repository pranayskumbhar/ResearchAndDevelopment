using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ResearchAndDevelopment.Controllers
{
    [Route("api")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _config;
        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        [Route("token")]
        public IActionResult GenerateJwtToken([FromBody] JsonElement root)
        {
            if (root.ValueKind == JsonValueKind.Null)
                return BadRequest();
            var role = root.GetProperty("role").GetString()!;
            var userid = root.GetProperty("userid").GetString()!;

            var claims = new[]  {
                new Claim(ClaimTypes.Name, userid),
                new Claim(ClaimTypes.Role, role)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiresInMinues"])), // Set token expiration
                signingCredentials: credentials
            );



            JwtResponse jwtResponse = new JwtResponse();
            jwtResponse.JwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            jwtResponse.Type = "Bearer";
            jwtResponse.ExpiresinMinutes = _config["Jwt:ExpiresInMinues"];
            return Ok(jwtResponse);
        }
    }
    public class JwtResponse
    {
        [JsonPropertyName("JwtToken")]
        public string? JwtToken { get; set; }

        [JsonPropertyName("Type")]
        public string? Type { get; set; }

        [JsonPropertyName("ExpiresinMinutes")]
        public string? ExpiresinMinutes { get; set; }

    }
}
