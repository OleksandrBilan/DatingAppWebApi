using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Route("test")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TestController : Controller
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("success");
        }
    }
}
