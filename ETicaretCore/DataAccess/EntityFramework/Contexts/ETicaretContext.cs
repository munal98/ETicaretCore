using AppCore.DataAccess.Configs;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework.Contexts
{
    public class ETicaretContext : DbContext
    {
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<KullaniciDetayi> KullaniciDetaylari { get; set; }
        public DbSet<Rol> Roller { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Code-First yaklaşımı üzerinden veritabanını oluşturabilmek için geçici olarak tanımlanmıştır.
            // Eğer MVC uygulama projesi oluşturulursa ve bu projede appsettings.json dosyasında connection string tanımlanırsa aşağıdaki satıra gerek yoktur.

            string connectionString;

            // Windows Authentication
            //connectionString = "server=.\\SQLEXPRESS;database=BA_ETicaretCore;trusted_connection=true;multipleactiveresultsets=true;";

            // SQL Server Authentication
            //connectionString = "server=.\\SQLEXPRESS;database=BA_ETicaretCore;user id=sa;password=sa;multipleactiveresultsets=true;";
            //connectionString = "server=.;database=BA_ETicaretCore;user id=sa;password=123;multipleactiveresultsets=true;";

            // MVCWebUI'da Program.cs'de appsettings.json'dan set ettiğimiz connection string.
            connectionString = ConnectionConfig.ConnectionString;

            // Scaffolding işlemi sırasında connectionString MVCWebUI Program.cs üzerinden set edilemediği için 
            // connection string'i aşağıda development veritabanı için set ediyoruz.
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                //connectionString = "server=.\\SQLEXPRESS;database=BA_ETicaretCore;user id=sa;password=sa;multipleactiveresultsets=true;";
                connectionString = "server=.;database=BA_ETicaretCore;user id=sa;password=123;multipleactiveresultsets=true;";
            }

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // EntityFramework FluentAPI configuration:

            // Tablolar arası ilişkiler yabancıl anahtar (foreign key) hangi entity'lerde varsa bu entity'ler üzerinden tanımlanır.
            // KategoriId foreign key'i Urun entity'sinde olduğu için aşağıdaki kısım Urun entity'si üzerinden yazılmalıdır.
            modelBuilder.Entity<Urun>()
                .ToTable("ETicaretUrunler")
                .HasOne(urun => urun.Kategori)
                .WithMany(kategori => kategori.Urunler)
                .HasForeignKey(urun => urun.KategoriId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Kullanici>()
                .ToTable("ETicaretKullanicilar")
                .HasOne(kullanici => kullanici.KullaniciDetayi)
                .WithOne(kullaniciDetayi => kullaniciDetayi.Kullanici)
                .HasForeignKey<Kullanici>(kullanici => kullanici.KullaniciDetayiId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Kullanici>()
                .HasOne(kullanici => kullanici.Rol)
                .WithMany(rol => rol.Kullanicilar)
                .HasForeignKey(kullanici => kullanici.RolId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Kategori>()
                .ToTable("ETicaretKategoriler");
            modelBuilder.Entity<KullaniciDetayi>()
                .ToTable("ETicaretKullaniciDetaylari");
            modelBuilder.Entity<Rol>()
                .ToTable("ETicaretRoller");
        }
    }
}
