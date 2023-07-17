using Yofi_ASP_Net.DataBase;
using Yofi_ASP_Net.Global;

namespace Yofi_ASP_Net.Interfaces
{
    public class EmbarkationResponse_OBJ<T>
    {
        public EmbarkationResponse Embar { get; set; }
        public T Obj { get; set; }
    }
    public interface EditoDto
    {
        public string? JWT { get; set; }
        public EmbarkationResponse Update(ref MainContext db);
        public EmbarkationResponse Create(ref MainContext db);
        public EmbarkationResponse Delete(ref MainContext db);
    }
    public interface EnquiryDto<T, J>
    {
        public EmbarkationResponse_OBJ<T> GetById(ref MainContext db, int Id);
        public EmbarkationResponse_OBJ<J> GetList(ref MainContext db, GetFromTo fromTo);
    }

}
