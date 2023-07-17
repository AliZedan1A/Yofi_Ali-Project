using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yofi_ASP_Net.Global;
using Yofi_ASP_Net.DataBase;
using Yofi_ASP_Net.Models;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using Google.Api;
using Newtonsoft.Json;

namespace Yofi_ASP_Net.Controllers.API
{
    public class FromTo
    {
        public int From { get; set; }
        public int To { get; set; }
    }
    [ApiController]
    [Route("/Products")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController>? _logger;
        private MainContext db = new MainContext();

        [HttpGet("ShowAllProducts")]
        public IActionResult Products([FromBody] ProductsModelDto req)
        {
           var russlt = req.GetList(ref db, new GetFromTo() { From = 0, To = db.Products.Count()-1 });
            if(russlt.Embar.IsDone==true)
            {
                return Ok(russlt);

            }
            else
            {
                return BadRequest(russlt);
            }

        }
        [HttpPost("AddProduct")]
        public IActionResult AddProducts([FromForm] ProductsModelDto req)
        {
            var russlt = req.Create(ref db);
            if (russlt.IsDone == true)
            {
                //Product main Image
                MemoryStream memory = new MemoryStream();
                req.MainImage.CopyTo(memory);
                ImageSetting img = new ImageSetting() { Image=memory,Format= Path.GetExtension(req.MainImage.FileName)};
                db.SaveChanges();
                var last = db.Products.SingleOrDefault(x => x.Name == req.Name);
                Image ProductImage = new Image(@"wwwroot/images", Image.Things.Products, last.Id);
                var ret =  ProductImage.Embarkation();
                Console.WriteLine(JsonConvert.SerializeObject(ret));
                 var z= ProductImage.InsertImage(img.GetImage(), true);
                if(!z.IsDone)
                {
                    db.Products.Remove(last);
                }
                //Product Images
                foreach (var item in req.Images)
                {
                    MemoryStream ImagesMemory = new MemoryStream();
                    item.CopyTo(ImagesMemory);
                    ImageSetting imgs = new ImageSetting() { Image = ImagesMemory, Format = Path.GetExtension(item.FileName) };
                    var rus = ProductImage.InsertImage(imgs.GetImage());
                    if (!rus.IsDone)
                    {
                        db.Products.Remove(last);
                    }

                }
                return Ok(russlt);

            }
            else
            {
                return BadRequest(russlt);
            }



        }
        [HttpDelete("RemoveProduct")]
        public IActionResult RemoveProducts([FromForm] ProductsModelDto req)
        {
            
            var z=req.Delete(ref db);
            if (z.IsDone)
            {
                db.SaveChanges();

                return Ok(z);

            }
            else
            {
                return BadRequest(z);
            }
        }
        [HttpPost("ModfyProduct")]
        public IActionResult ModfyProducts([FromForm] ProductsModelDto req)
        {
            var russlt = req.Update(ref db);
            if(russlt.IsDone)
            {
                return Ok(russlt);

            }
            else
            {
                return BadRequest(russlt);
            }
        }

    }
}
