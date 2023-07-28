using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UrunModel : RecordBase
    {
        #region Entity'den gelen özellikler
        [Required(ErrorMessage = "{0} zorunludur!")]
        [MinLength(2, ErrorMessage = "{0} minimum {1} karakter olmalıdır!")]
        [MaxLength(200, ErrorMessage = "{0} maksimum {1} karakter olmalıdır!")]
        [DisplayName("Ürün Adı")]
        public string? Adi { get; set; }

        [StringLength(500, ErrorMessage = "{0} maksimum {1} karakter olmalıdır!")]
        [DisplayName("Açıklaması")]
        public string? Aciklamasi { get; set; }

        [DisplayName("Birim Fiyatı")]
        public double BirimFiyati { get; set; }

        [Required(ErrorMessage = "{0} zorunludur!")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} {1} ile {2} aralığında olmalıdır!")]
        [DisplayName("Stok Miktarı")]
        public int StokMiktari { get; set; }

        [DisplayName("Son Kullanma Tarihi")]
        public DateTime? SonKullanmaTarihi { get; set; }

        [DisplayName("Kategori")]
        [Required(ErrorMessage = "{0} zorunludur!")]
        public int KategoriId { get; set; }
        #endregion

        [DisplayName("Birim Fiyatı")]
        public string? BirimFiyatiModel { get; set; }

        [DisplayName("Son Kullanma Tarihi")]
        public string? SonKullanmaTarihiModel { get; set; }

        public KategoriModel? KategoriModel { get; set; }

        [StringLength(255, ErrorMessage = "{0} en fazla {1} karakter olmalıdır!")]
        [DisplayName("İmaj")]
        public string? ImajDosyaAdi { get; set; }
    }
}
