using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("test")]
    public class TestController : Controller
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("success");
        }
    }
}
