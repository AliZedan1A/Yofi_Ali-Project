using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Yofi_ASP_Net.DataBase;
using Yofi_ASP_Net.Global;
using Yofi_ASP_Net.Models;

namespace Yofi_ASP_Net.Controllers
{
    [ApiController]
    [Route("/User")]
    public class UserController : Controller
    {
        public static MainContext DB = new MainContext();
       
        [HttpPost("aa")]
        public IActionResult aa(ProductsModelDto r)
        {
            r.Update(ref DB);
            DB.SaveChanges(); ;
            return Ok();
        }

    }
}
