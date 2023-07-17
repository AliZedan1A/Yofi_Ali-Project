using Yofi_ASP_Net.DataBase;

namespace Yofi_ASP_Net.Global
{
    public class JWTData {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public Roles Role { get; set; }
    }
    public class Invoice_Product
    {
        public int Id { get; set; }
        public string Name  { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
    public enum Roles
    {
        Owner=1,
        admin=2,
        user=3
    }
    public class GetFromTo
    {
        public int From { get; set; }
        public int To { get; set; }
        public string Search { get; set; }
        public DateTime From_time { get; set; }
        public DateTime To_time { get; set; }
    }
}
