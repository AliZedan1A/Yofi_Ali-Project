using Yofi_ASP_Net.DataBase;
using Yofi_ASP_Net.Global;
using Yofi_ASP_Net.Interfaces;

namespace Yofi_ASP_Net.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Discord { get; set; }

    }
    public class MessageModelDto : EditoDto
    {
        public int? Id { get; set; }
        public string? Message { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Discord { get; set; }

        public string? JWT { get; set ; }

        public EmbarkationResponse Create(ref MainContext db)
        {
            throw new NotImplementedException();
        }

        public EmbarkationResponse Delete(ref MainContext db)
        {
            throw new NotImplementedException();
        }

        public EmbarkationResponse_OBJ<EmbarkationResponse> GetList(ref MainContext db, GetFromTo fromTo)
        {
            throw new NotImplementedException();
        }

        public EmbarkationResponse Update(ref MainContext db)
        {
            throw new NotImplementedException();
        }
    }

}
