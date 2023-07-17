using Microsoft.AspNetCore.Mvc;

namespace Yofi_ASP_Net.Controllers
{
    [ApiController]
    [Route("/Dashbord")]

    public class DashbordController : Controller
    {
        [HttpGet("Dashbord")]
        public IActionResult dash()
        {
            return Ok();
        }
    }
}
