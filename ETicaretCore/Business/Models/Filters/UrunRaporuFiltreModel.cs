using AppCore.Business.Validations;
using System.ComponentModel;

namespace Business.Models.Filters
{
    public class UrunRaporuFiltreModel
    {
        [DisplayName("Kategori")]
        public int? KategoriId { get; set; }

        [DisplayName("Ürün Adı")]
        public string? UrunAdi { get; set; }

        [DisplayName("Birim Fiyat")]
        [StringToDouble(ErrorMessage = "{0} sayısal olmalıdır!")]
        public string? BirimFiyatBaslangic { get; set; }

        [StringToDouble(ErrorMessage = "{0} sayısal olmalıdır!")]
        public string? BirimFiyatBitis { get; set; }

        [DisplayName("Stok Miktarı")]
        public int? StokMiktariBaslangic { get; set; }

        public int? StokMiktariBitis { get; set; }

        [DisplayName("Son Kullanma Tarihi")]
        public string? SonKullanmaTarihiBaslangic { get; set; }

        public string? SonKullanmaTarihiBitis { get; set; }
    }
}
