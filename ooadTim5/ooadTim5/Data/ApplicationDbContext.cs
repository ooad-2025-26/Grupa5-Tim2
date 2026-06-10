using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ooadTim5.Models;

namespace ooadTim5.Data
{
    public class ApplicationDbContext : IdentityDbContext, IDataProtectionKeyContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Knjiga> Knjige { get; set; }
        public DbSet<Posudba> Posudbe { get; set; }
        public DbSet<ZahtjevZaPosudbu> Zahtjevi { get; set; }
        public DbSet<NabavkaKnjiga> Nabavke { get; set; }
        public DbSet<Dobavljac> Dobavljaci { get; set; }
        public DbSet<ClanskaKartica> ClanskeKartice { get; set; }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Knjiga>().ToTable("Knjiga");
            modelBuilder.Entity<Posudba>().ToTable("Posudba");
            modelBuilder.Entity<ZahtjevZaPosudbu>().ToTable("ZahtjevZaPosudbu");
            modelBuilder.Entity<NabavkaKnjiga>().ToTable("NabavkaKnjiga");
            modelBuilder.Entity<Dobavljac>().ToTable("Dobavljac");
            modelBuilder.Entity<ClanskaKartica>().ToTable("ClanskaKartica");

            base.OnModelCreating(modelBuilder);
        }
    }
}