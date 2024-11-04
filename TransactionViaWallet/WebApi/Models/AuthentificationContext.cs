using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebAPI.Models;

namespace WebAPI.Models
{
    public class AuthenticationContext : IdentityDbContext
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }
        public DbSet<StockWalletModel> StockWallets { get; set; }
        public DbSet<WalletModel> Wallets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des clés composites
            modelBuilder.Entity<ApplicationUserModel>()
                .HasKey(u => new { u.BankId, u.PhoneNumber, u.CIN });
            modelBuilder.Entity<StockWalletModel>().ToTable("StockWallets");
        }

    }
}
