using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Yofi_ASP_Net.Controllers.API;
using Yofi_ASP_Net.DataBase;
using Yofi_ASP_Net.Global;
using Yofi_ASP_Net.Interfaces;

namespace Yofi_ASP_Net.Models
{
    public class OrdersModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Buyer_Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Products{ get; set; }
    }
    public class OrdersModelDto : EditoDto, EnquiryDto<OrdersModel, IQueryable<OrdersModel>>
    { 
        public int? Id { get; set; }
        public int? Buyer_Id { get; set; }
        public DateTime? Date { get; set; }
        public List<Invoice_Product>? ProductsList { get; set; }
        public string? JWT { get ; set; }

        public EmbarkationResponse Create(ref MainContext db)
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
            OrdersModel Order = new();
            if (Buyer_Id is null || Date is null || ProductsList is null)
            {
                return new EmbarkationResponse() { Msg = "Missing Params", IsDone = false };
            }
            Order.Products=JsonConvert.SerializeObject(ProductsList);
            Order.Buyer_Id = (int)Buyer_Id;
            Order.Date = (DateTime)Date;
            db.Orders.Add(Order);
            return new EmbarkationResponse() { IsDone = true, Msg = "Done Add Order" };
        }
        public EmbarkationResponse_OBJ<OrdersModel> GetById(ref MainContext db,int Id)
        {
            var data = db.Orders.SingleOrDefault(x => x.Id == Id);
           return new EmbarkationResponse_OBJ<OrdersModel>()
            {
                Embar = new EmbarkationResponse() { IsDone = true, Msg = "done" },
                Obj = data
            };
        }

        public EmbarkationResponse_OBJ<IQueryable<OrdersModel>> GetList(ref MainContext db , GetFromTo fromTo)
        {
            var jwt = JwtAuthManager.IsJWTOk(JWT);
            if (!jwt.Embar.IsDone)
            {
                return new EmbarkationResponse_OBJ<IQueryable<OrdersModel>>()
                {
                    Embar = jwt.Embar,
                    Obj = null
                };
            }
            IQueryable<OrdersModel> list;
            int from = fromTo.From;
            int to = fromTo.To;
            from--;
            if (from <= 0 || from > to)
            {
                from = 0;
            }
            var isOwner = jwt.Obj.Role == Roles.Owner;
            if (fromTo.Search.IsNullOrEmpty())
            {
                list = db.Orders.Where(x => isOwner ? true : x.Id == Id).Skip(from).Take(to);
            }
            else
            {
                list = db.Orders.Where(x => (isOwner ? true : x.Id == Id) && EF.Functions.Like(x.Products, $"%{fromTo.Search}%")).Skip(from).Take(to);
            }
            return new EmbarkationResponse_OBJ<IQueryable<OrdersModel>>()
            {
                Embar = new EmbarkationResponse() { Msg = "done", IsDone = true },
                Obj = list
            };
        }

        public EmbarkationResponse Delete(ref MainContext db)
        {
            // no delete
            throw new NotImplementedException();

        }

         public EmbarkationResponse Update(ref MainContext db)
        {
            // no update
            throw new NotImplementedException();
        }

    }
}
