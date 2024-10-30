using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bank_Application.Data
{
    public class BankContext : DbContext
    {
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<UserBankInfo> UserBankInfos { get; set; }

        public DbSet<UserIBANInfo> UserIBANInfos { get; set; }
        public DbSet<CreditBooleanInfo> CreditBooleanInfos { get; set; }
        public DbSet<CreditDateInfo> CreditDateInfos{ get; set; }
        public DbSet<CreditMoneyInfo> CreditMoneyInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string jsonFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "GitSecrets.json");
            string json = File.ReadAllText(jsonFilePath);
            JsonElement root = JsonDocument.Parse(json).RootElement;

            string? connectionString = root.GetProperty("ConnectionStrings").GetProperty("BankApplication").GetString();
            optionsBuilder.UseMySQL(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
