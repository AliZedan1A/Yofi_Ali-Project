namespace Yofi_ASP_Net.Global
{
    public class Config
    {
       public static readonly string DB_Host = "localhost";
       public static readonly string DB_Username = "root";
       public static readonly string DB_Password = "";
       public static readonly string DB_Name = "project";
       public static readonly Version DB_Version = new Version(10, 4, 17);
    }


    public class JwtSetting
    {
        public static readonly string JWT_SECURITY_KEY = "rashwer34534834yry43ehuhg8934y328yrehfhf9834yhtrfrtet34ed"; //لازم يكون فوق ال60 علامه او احرف او ارقام
    }

    public class Res
    {
        public int status { get; set; }
        public string? msg { get; set; }
        public object? Data { get; set; }
    }


}
