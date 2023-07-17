using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Yofi_ASP_Net.DataBase;
using Yofi_ASP_Net.Global;
using Yofi_ASP_Net.Interfaces;

namespace Yofi_ASP_Net.Models
{
    public class modfiyimage
    {
        public int Imageid { get; set; }
        public IFormFile Image { get; set; }
    }
    [Index(nameof(Name), IsUnique = true)]
    public class ProductsModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Catigory_Id { get; set; }
        [Required]
        [MaxLength(35)]
        public string Name { get; set; }
        [Required]
        [MaxLength(8)]
        public double? Price { get; set; }
        [Required]
        [MaxLength(350)]
        public string Discription { get; set; }
    }
    public class ProductsModelDto: EditoDto , EnquiryDto<ProductsModel, ProductsModel>
    {
        public int? Id { get; set; }
        public IFormFile? MainImage { get; set; }
        public List<IFormFile>? Images { get; set; }
        public int? Catigory_Id { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public string? Discription { get; set; }
        public string? JWT { get; set; }
        public IFormFile? MainImageModfiy { get; set; }
        public List<modfiyimage>? ProductsImagesModfiy { get; set; }

        public EmbarkationResponse Create(ref MainContext db)
        {
            if(db.Catigoriess.SingleOrDefault(c => c.Id == Catigory_Id)is null)
            {
                return new EmbarkationResponse() { Msg = $"There is no catigory with id {Catigory_Id}", IsDone = false };
            }
            if (JWT is null)
            {
                return new EmbarkationResponse() { Msg = "jwt null", IsDone = false };
            }
            var jwtsession = JwtAuthManager.Get(JWT);
            if (jwtsession.IsValid == false)
            {

                return new EmbarkationResponse() { IsDone = false, Msg = "Jwt Expires" };
            }
            Console.WriteLine(JsonConvert.SerializeObject( jwtsession));
            ProductsModel Product = new();
            if (Name is null || Price is null || Discription is null || Catigory_Id is null)
            {
                return new EmbarkationResponse() { Msg = "Missing Params", IsDone = false };
            }
            Product.Name = Name;
            Product.Price = Price;
            Product.Discription = Discription;
            Product.Catigory_Id = (int)Catigory_Id;

            db.Products.Add(Product);
            return new EmbarkationResponse() { IsDone = true, Msg = "Done Add product" };
        }

        public EmbarkationResponse Delete(ref MainContext db)
        {
            var jwt = JwtAuthManager.IsJWTOk(JWT);
            if(jwt.Obj.Role== Roles.admin||jwt.Obj.Role==Roles.Owner) {

                return new EmbarkationResponse()
                {
                    IsDone = false,
                    Msg = "You don't have Role to delete Product"
                };
            }

            if (!jwt.Embar.IsDone)
            {
                return new EmbarkationResponse()
                {
                    IsDone = false,
                    Msg = jwt.Embar.Msg
                };
            }
            var product = db.Products.Where(x => x.Id == Id);//.ExecuteDelete();
            if (product.Count() == 0)
            {
                return new EmbarkationResponse() { Msg = "Products not found", IsDone = false };

            }
            Image img = new Image("wwwroot/images", Image.Things.Products, product.First().Id);
            img.Embarkation();
            var russlt = img.RemoveImage();
            if (russlt.Msg == "Product Not Found in Images") goto l;
            if(!russlt.IsDone) return new EmbarkationResponse() { Msg = russlt.Msg, IsDone = false };
            l:;
            var Catigory = product.ExecuteDelete();
            if (Catigory == 0)
            {
                return new EmbarkationResponse() { Msg = "Products not found", IsDone = false };
            }
            return new EmbarkationResponse() { Msg = "Products Deleted", IsDone = true };

        }

        public EmbarkationResponse_OBJ<ProductsModel> GetById(ref MainContext db, int Id)
        {
            var Product = db.Products.SingleOrDefault(x => x.Id == Id);
            if (Product == null)
            {
                return new EmbarkationResponse_OBJ<ProductsModel>()
                {
                    Embar = new EmbarkationResponse() { Msg = "Product not found", IsDone = false },
                    Obj = null
                };
            }
            return new EmbarkationResponse_OBJ<ProductsModel>()
            {
                Embar = new EmbarkationResponse() { Msg = "Product found", IsDone = false },
                Obj = Product
            };
        }


        public EmbarkationResponse_OBJ<ProductsModel> GetList(ref MainContext db, GetFromTo fromTo)
        {
            throw new NotImplementedException();
        }

        public EmbarkationResponse Update(ref MainContext db)
        {
            if (JWT is null)
            {
                return new EmbarkationResponse() { Msg = "missing params", IsDone = false };
            }
            var jwtsession = JwtAuthManager.Get(JWT);
            if (jwtsession.IsValid == false)
            {
                return new EmbarkationResponse() { IsDone = false, Msg = "Jwt Expires" };
            }
            var Product = db.Products.SingleOrDefault(x => x.Id == Id);
            if (Product == null)
            {
                return new EmbarkationResponse() { Msg = "missing params", IsDone = false };
            }
            var isDone = false;
            string Msg = "Update Values : ";
            if (this.Name is not null)
            {
                Msg += "User ";
                isDone = true;
                Product.Name = this.Name;
            }
            if (this.Price is not null)
            {
                Msg += "Price ";
                isDone = true;
                Product.Price = this.Price;
            }
            if (this.Discription is not null)
            {
                Msg += "Discription ";
                isDone = true;
                Product.Discription = this.Discription;
            }
            if (this.MainImageModfiy is not null)
            {
                Msg += "MainImage ";
                MemoryStream memory = new MemoryStream();
                MainImageModfiy.CopyTo(memory);
                ImageSetting img = new ImageSetting() { Image = memory, Format = Path.GetExtension(MainImageModfiy.FileName) };
                Image ProductImage = new Image(@"wwwroot/images", Image.Things.Products, (int)Id);
                var ret = ProductImage.Embarkation();
                Console.WriteLine(JsonConvert.SerializeObject(ret));
                var z = ProductImage.InsertImage(img.GetImage(), true);
                if(!z.IsDone) 
                {
                return new EmbarkationResponse() { Msg = z.Msg, IsDone = false };
                }
                isDone = true;
            }
            return new EmbarkationResponse() { Msg = Msg, IsDone = isDone };
        }
    }
}
