using Google.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Runtime.CompilerServices;
using Yofi_ASP_Net.DataBase;
using Yofi_ASP_Net.Global;
using Yofi_ASP_Net.Interfaces;

namespace Yofi_ASP_Net.Models
{
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]

    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(35)]
        public string Name { get; set; }
        [Required]
        [MaxLength(35)]
        public string Password { get; set; }
        [Required]
        [MaxLength(35)]
        public string Email { get; set; }
        [Required]
        [MaxLength(5)]
        public int Role { get; set; }
        [MaxLength(35)]
        public string Phone { get; set; }
        [ForeignKey("Buyer_Id")]
        public ICollection<OrdersModel>? Orders { get; set; }

    }
    public class UserModeldto : EditoDto, EnquiryDto<UserModel, IQueryable<UserModel>>
    {
        public int? Role { get; set; }
        [MaxLength(35)]
        public string? Name { get; set; }
        [MaxLength(35)]
        public string? Password { get; set; }
        public IFormFile? Image_Data { get; set; }
        [MaxLength(35)]
        public string? Email { get; set; }
        [MaxLength(10000)]
        public string? JWT { get; set; }
        [MaxLength(35)]
        public string? Phone { get; set; }

        public EmbarkationResponse Create(ref MainContext db)
        {
            if(Email is null || Name is null || Password is null)
            {
                return new EmbarkationResponse() { Msg = "missing params", IsDone = false };
            }
            if(Image_Data is not null)
            {
                //MemoryStream memory = new MemoryStream();
                //Image_Data.CopyTo(memory);
                //System.Drawing.Image x = (Bitmap)((new ImageConverter()).ConvertFrom(memory.ToArray()));
                //x.Save($@"path", format: System.Drawing.Imaging.ImageFormat.Png);
            }
            var user = new UserModel()
            {
                Email = Email!,
                Name = Name!,
                Role = (int) Role,
                Password = Hasher.Hash(Password!),
                Phone = Phone
            };
            db.Add(user);
            return new EmbarkationResponse() { Msg = "User Created !!!", IsDone = false };
        }

        public EmbarkationResponse Delete(ref MainContext db)
        {
            throw new NotImplementedException();
        }

        public EmbarkationResponse_OBJ<UserModel> GetById(ref MainContext db, int Id)
        {
            var user = db.Users.SingleOrDefault(x => x.Id == Id);
            return new EmbarkationResponse_OBJ<UserModel>()
            {
                Embar = new()
                {
                    IsDone = true,
                    Msg = "done",
                },
                Obj = user
            };
        }
        public UserModel GetByNameOrEmail_And_Password(ref MainContext db, string email_or_Username)
        {
            var user =  db.Users.SingleOrDefault(x => x.Email == email_or_Username || x.Name == email_or_Username);
            return user;
        }
        public UserModel GetByNameOrEmail_No_Password(ref MainContext db, string email_or_Username)
        {
            var user = db.Users.SingleOrDefault(x => x.Email == email_or_Username || x.Name == email_or_Username);
            return new UserModel()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone,
                Password = null,
                Role = user.Role
            };
            
        }
        public EmbarkationResponse_OBJ<IQueryable<UserModel>> GetList(ref MainContext db, GetFromTo fromTo)
        {
            //List<UserModel> list = new List<UserModel>();
            var d = JwtAuthManager.IsJWTOk(JWT);
            if (!d.Embar.IsDone)
            {
                return new EmbarkationResponse_OBJ<IQueryable<UserModel>>()
                {
                    Embar = d.Embar,
                    Obj = null,
                };
            }
                IQueryable<UserModel> list;
                int from = fromTo.From;
                int to = fromTo.To;
                from--;
                if (from <= 0 || from > to)
                {
                    from = 0;
                }

                if (fromTo.Search.IsNullOrEmpty())
                {
                    list = db.Users.Where(b => true).Skip(from).Take(to);
                }
                else
                {
                    list = db.Users.Where(b => EF.Functions.Like(b.Name, $"%{fromTo.Search}%")).Skip(from).Take(to);
                }



                return new EmbarkationResponse_OBJ<IQueryable<UserModel>>()
                {
                    Embar = new EmbarkationResponse() { Msg = "done", IsDone = true },
                    Obj = list
                };
            
        }

        //admin override
        public EmbarkationResponse Update(ref MainContext db, int id)
        {
            var d = JwtAuthManager.IsJWTOk(JWT);
            if (!d.Embar.IsDone)
            {
                return new EmbarkationResponse() { Msg = "missing params", IsDone = false };
            }
            var jwtsession = JwtAuthManager.Get(JWT);
            if (jwtsession.IsValid == false)
            {
                return new EmbarkationResponse() { IsDone = false, Msg = "Jwt Expires" };
            }
            if(jwtsession.Data.Role != Roles.Owner )
            {
                return new EmbarkationResponse() { IsDone = false, Msg = "Invalid Permetion" };
            }
            var user = db.Users.SingleOrDefault(x => x.Id == id);
            if (user == null)
            {
                return new EmbarkationResponse() { Msg = "missing params", IsDone = false };
            }
            var isDone = false;
            string Msg = "Update Values : ";
            if (this.Name is not null)
            {
                Msg += "User ";
                isDone = true;
                user.Name = this.Name;
            }
            if (this.Role is not null)
            {
                Msg += "Role ";
                isDone = true;
                user.Role = (int)this.Role;
            }
            if (this.Password is not null)
            {
                Msg += "Password ";
                isDone = true;
                user.Password = Hasher.Hash(this.Password);
            }
            if (this.Email is not null)
            {
                Msg += "Email ";
                isDone = true;
                user.Email = this.Email;
            }
            return new EmbarkationResponse() { Msg = Msg, IsDone = isDone };
        }
        public  EmbarkationResponse Update(ref MainContext db)
        {
            if(JWT is null)
            {
                return new EmbarkationResponse() { Msg = "missing params", IsDone = false };
            }
            var jwtsession =JwtAuthManager.Get(JWT);
            if (jwtsession.IsValid == false)
            {
                return new EmbarkationResponse() { IsDone = false, Msg = "Jwt Expires" };
            }

            int? id = jwtsession.Data.UserId;
            var user = db.Users.SingleOrDefault(x => x.Id == id);
            if(user == null)
            {
                return new EmbarkationResponse() { Msg= "missing params", IsDone=false};
            }
            var isDone = false;
            string Msg = "Update Values : ";
            if(this.Name is not null)
            {
                Msg += "User ";
                isDone = true;
                user.Name = this.Name;
            }
            if (this.Password is not null)
            {
                Msg += "Password ";
                isDone = true;
                user.Password = this.Password;
            }
            if (this.Email is not null)
            {
                Msg += "Email ";
                isDone = true;
                user.Email = this.Email;
            }
            return new EmbarkationResponse() { Msg = Msg, IsDone = isDone };

        }

     
    }

}
