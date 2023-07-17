using Microsoft.AspNetCore.Mvc;

namespace Yofi_ASP_Net.Controllers
{
    [ApiController]
    [Route("/Auth")]
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult lgn()
        {
            return Ok();
        }
    }
}
 