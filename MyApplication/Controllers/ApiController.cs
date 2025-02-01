using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ResearchAndDevelopment.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ApiController : ControllerBase
    {
        [HttpGet]
        [Route("Home")]
        public IActionResult Home()
        {
            // Extract name and role from the JWT token
            var name = User.Identity.Name;
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var resp = new
            {
                name = name,
                role = role
            };

            return Ok(resp);
        }
    }
}
