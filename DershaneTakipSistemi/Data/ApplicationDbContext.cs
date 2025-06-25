using DershaneTakipSistemi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DershaneTakipSistemi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ogrenci> Ogrenciler { get; set; }
        // public DbSet<Odeme> Odemeler { get; set; } // <-- BU SATIR SİLİNDİ
        public DbSet<Personel> Personeller { get; set; }
        public DbSet<Sinif> Siniflar { get; set; }
        public DbSet<KasaHareketi> KasaHareketleri { get; set; } // <-- Bu satır önceki adımda eklenmişti

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Odeme entity'si ile ilgili olan ve aşağıdakine benzer blok SİLİNDİ
            /*
            modelBuilder.Entity<Odeme>() 
                .HasOne(o => o.Ogrenci) 
                .WithMany(p => p.Odemeler) 
                .HasForeignKey(o => o.OgrenciId) 
                .OnDelete(DeleteBehavior.Restrict);
            */
        }
    }
}