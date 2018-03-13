using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using _42Data.Investiments.Web.Models;
using _42Data.Investiments.Web.Models.Wallet;
using _42Data.Investiments.Web.Models.Withdraw;

namespace _42Data.Investiments.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<WalletCliente> WalletCliente { get; set; }
        public DbSet<WalletWithdraw> Walletwithdraw { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            WalletClientBuilder(builder);

            WalletWithdrawBuilder(builder);
            
            base.OnModelCreating(builder);
        }

        public void WalletClientBuilder(ModelBuilder builder)
        {
            builder.Entity<WalletCliente>()
                .Property(x => x.ValorInicial)
                .HasColumnType("decimal(18,4)");

            builder.Entity<WalletCliente>()
                .Property(x => x.ValorEntradaWise)
                .HasColumnType("decimal(18,4)");

            builder.Entity<WalletCliente>()
                .Property(x => x.ValorEntradaBold)
                .HasColumnType("decimal(18,4)");

            builder.Entity<WalletCliente>()
                .Property(x => x.ValorRentabilidadeWise)
                .HasColumnType("decimal(18,4)");

            builder.Entity<WalletCliente>()
                .Property(x => x.ValorRentabilidadeBold)
                .HasColumnType("decimal(18,4)");

            builder.Entity<WalletCliente>()
                .Property(x => x.ValorFinal)
                .HasColumnType("decimal(18,4)");

            builder.Entity<WalletCliente>()
                .Property(x => x.ValorComissaoTotal)
                .HasColumnType("decimal(18,4)");
        }

        public void WalletWithdrawBuilder(ModelBuilder builder)
        {
            builder.Entity<WalletWithdraw>()
               .Property(x => x.ValorBold)
               .HasColumnType("decimal(18,4)");

            builder.Entity<WalletWithdraw>()
                .Property(x => x.ValorWise)
                .HasColumnType("decimal(18,4)");
        }
    }
}
