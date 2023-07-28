using AppCore.Records.Bases;
using System.ComponentModel;

namespace Business.Models.Reports
{
    public class UrunRaporuModel : RecordBase
    {
        #region Sayfada Gösterilecekler
        [DisplayName("Ürün Adı")]
        public string? UrunAdi { get; set; }

        [DisplayName("Kategori Adı")]
        public string? KategoriAdi { get; set; }

        [DisplayName("Kategori Açıklama")]
        public string? KategoriAciklamasi { get; set; }

        [DisplayName("Birim Fiyatı")]
        public string? BirimFiyatiModel { get; set; }

        [DisplayName("Stok Miktarı")]
        public int StokMiktari { get; set; }

        [DisplayName("Son Kullanma Tarihi")]
        public string? SonKullanmaTarihiModel { get; set; }

        [DisplayName("Oluşturulma Tarihi")]
        public string? CreateDateModel { get; set; }

        [DisplayName("Güncellenme Tarihi")]
        public string? UpdateDateModel { get; set; }

        [DisplayName("Oluşturan")]
        public string? CreatedByModel { get; set; }

        [DisplayName("Güncelleyen")]
        public string? UpdatedByModel { get; set; }
        #endregion

        #region Filtreleme için
        public int? KategoriId { get; set; }
        public double BirimFiyat { get; set; }
        public DateTime? SonKullanmaTarihi { get; set; }
        #endregion
    }
}
