using Microsoft.AspNetCore.Mvc;

namespace Yofi_ASP_Net.Controllers
{
    [ApiController]
    [Route("/Orders")]
    public class OrdersController : Controller
    {
        [HttpGet]
        public IActionResult ord()
        {
            return Ok();
        }
    }
}
