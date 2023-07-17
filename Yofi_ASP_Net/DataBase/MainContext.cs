using Microsoft.EntityFrameworkCore;
using Yofi_ASP_Net.Models;
using Yofi_ASP_Net.Global;

namespace Yofi_ASP_Net.DataBase
{
    public class MainContext : DbContext
    {
      

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql(connectionString: $@"datasource={Config.DB_Host};username={Config.DB_Username};password={Config.DB_Password};database={Config.DB_Name};SslMode=none", new MySqlServerVersion(Config.DB_Version));
        }
        public DbSet<CatigoriessModel> Catigoriess { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<OrdersModel> Orders { get; set; }

        public DbSet<ProductsModel> Products { get; set; }
    }
}
