using Microsoft.EntityFrameworkCore;
using UATP.RapidPay.Data.Models;

namespace UATP.RapidPay.Data
{
    public class RapidPayDbContext(DbContextOptions<RapidPayDbContext> options) : DbContext(options)
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>().Property(x => x.Balance).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Payment>().Property(x => x.Amount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Payment>().Property(x => x.Fee).HasColumnType("decimal(18,2)");
        }
    }
}
