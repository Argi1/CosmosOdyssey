using CosmosOdyssey.Models;
using Microsoft.EntityFrameworkCore;

namespace CosmosOdyssey.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Leg> Legs { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Pricelist> Pricelists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("Server = db; Database = db; Uid = root; Pwd = password;");
            //optionsBuilder.UseMySQL("Server = 127.0.0.1; Database = db; Uid = root; Pwd = password;");
        }
    }
}
