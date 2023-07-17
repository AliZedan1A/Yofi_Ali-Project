using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Yofi_ASP_Net.DataBase;
using Yofi_ASP_Net.Global;
using Yofi_ASP_Net.Interfaces;

namespace Yofi_ASP_Net.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class CatigoriessModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(35)]
        public string Name { get; set; }
        [ForeignKey("Catigory_Id")]
        public ICollection<ProductsModel>? ProductsModel { get; set; }
    }
    public class CatigoryModeldto : EditoDto, EnquiryDto<CatigoriessModel, IQueryable<ProductsModel>>
    {
        [Required]
        [MaxLength(35)]
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? JWT { get; set; }

        public EmbarkationResponse Create(ref MainContext db)
        {
            var jwt = JwtAuthManager.IsJWTOk(JWT, Roles.Owner);
            if (!jwt.Embar.IsDone)
            {
                return new EmbarkationResponse()
                {
                    IsDone = false,
                    Msg = jwt.Embar.Msg
                };
            }
            db.Catigoriess.Add(new CatigoriessModel()
            {
                Name = Name,
            }
            );
            return new EmbarkationResponse()
            {
                IsDone = true,
                Msg = "User created"
            };
        }

        public EmbarkationResponse_OBJ<CatigoriessModel> GetById(ref MainContext db, int Id)
        {
            throw new NotImplementedException();
        }

        public EmbarkationResponse_OBJ<IQueryable<ProductsModel>> GetList(ref MainContext db, GetFromTo fromTo)
        {
            var jwt = JwtAuthManager.IsJWTOk(JWT);
            if (!jwt.Embar.IsDone)
            {
                return new EmbarkationResponse_OBJ<IQueryable<ProductsModel>>()
                {
                    Embar = jwt.Embar,
                    Obj = null
                };
            }

            var WorkingCatigory = db.Catigoriess.SingleOrDefault(x => x.Id == Id);
            if(WorkingCatigory == null)return new EmbarkationResponse_OBJ<IQueryable<ProductsModel>>()
            {
                Embar = new EmbarkationResponse()
                {
                    IsDone = false,
                    Msg = "Id not found"
                },
                Obj = null
            };
            IQueryable<ProductsModel> list;
            int from = fromTo.From;
            int to = fromTo.To;
            from--;
            if (from <= 0 || from > to)
            {
                from = 0;
            }

            if (fromTo.Search.IsNullOrEmpty())
            {
                list = db.Products.Where(b => b.Catigory_Id==WorkingCatigory.Id).Skip(from).Take(to);
            }
            else
            {
                list = db.Products.Where(b => (b.Catigory_Id == WorkingCatigory.Id) && EF.Functions.Like(b.Name ,$"%{fromTo.Search}%")).Skip(from).Take(to);
            }


            return new EmbarkationResponse_OBJ<IQueryable<ProductsModel>>()
            {
                Embar = new EmbarkationResponse()
                {
                    IsDone = true,
                    Msg = "Id not found"
                },
                Obj = list
            };
        }
        public EmbarkationResponse_OBJ<List<CatigoryModeldto>> GetAllCatigorys(ref MainContext db)
        {
            var jwt = JwtAuthManager.IsJWTOk(JWT);
            if (!jwt.Embar.IsDone)
            {
                return new EmbarkationResponse_OBJ<List<CatigoryModeldto>>()
                {
                    Embar = new EmbarkationResponse()
                    {
                        IsDone = false,
                        Msg = jwt.Embar.Msg
                    },
                    Obj = null
                };
            }

            List<CatigoryModeldto> ctglist = new List<CatigoryModeldto>();
            foreach (var item in db.Catigoriess.ToList())
            {
                CatigoryModeldto c = new CatigoryModeldto();
                c.Name = item.Name;
                c.Id = item.Id;
                ctglist.Add( c );
            }
            return new EmbarkationResponse_OBJ<List<CatigoryModeldto>>()
            {
                Embar = new EmbarkationResponse()
                {
                    IsDone = false,
                    Msg = jwt.Embar.Msg
                },
                Obj = ctglist
            };
        }



        public EmbarkationResponse Update(ref MainContext db)
        {
            var jwt = JwtAuthManager.IsJWTOk(JWT, Roles.Owner);
            if (!jwt.Embar.IsDone)
            {
                return new EmbarkationResponse()
                {
                    IsDone = false,
                    Msg = jwt.Embar.Msg
                };
            }
            var Catigory = db.Catigoriess.SingleOrDefault(x => x.Id == Id);
            if (Catigory == null)
            {
                return new EmbarkationResponse() { Msg = "Catogary Not Found", IsDone = false };
            }
            var isDone = false;
            string Msg = "Update Values : ";
            if (this.Name is not null)
            {
                Msg += "Name ";
                isDone = true;
                Catigory.Name = this.Name;
            }

            return new EmbarkationResponse() { Msg = Msg, IsDone = isDone };
        }

        public EmbarkationResponse Delete(ref MainContext db)
        {
            var jwt = JwtAuthManager.IsJWTOk(JWT, Roles.Owner);
            if (!jwt.Embar.IsDone)
            {
                return new EmbarkationResponse()
                {
                    IsDone = false,
                    Msg = jwt.Embar.Msg
                };
            }
            int Catigory = db.Catigoriess.Where(x => x.Id == Id).ExecuteDelete();
            if (Catigory == 0)
            {
                return new EmbarkationResponse() { Msg = "Catogary not found", IsDone = false };
            }
            return new EmbarkationResponse() { Msg = "Catogary Deleted", IsDone = true };
        }
    }
}



