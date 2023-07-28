using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities
{
    //[Table("ETicaretUrunler")] // ETicaretContext class'ında OnModelCreating methodunda tablo ismini özelleştirmek daha uygundur.
    public class Urun : RecordBase
    {
        [Required]
        [StringLength(200)]
        public string? Adi { get; set; }

        [StringLength(500)]
        public string? Aciklamasi { get; set; }

        public double BirimFiyati { get; set; }

        public int StokMiktari { get; set; }

        public DateTime? SonKullanmaTarihi { get; set; }

        public int KategoriId { get; set; }

        public Kategori? Kategori { get; set; }

        [StringLength(255)]
        public string? ImajDosyaAdi { get; set; }
    }
}
