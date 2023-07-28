namespace Business.Models
{
    public class SepetGroupByModel
    {
        public int UrunId { get; set; }
        public int KullaniciId { get; set; }

        public string? UrunAdi { get; set; }

        public double BirimFiyatToplami { get; set; }

        public int ToplamUrunSayisi { get; set; }
    }
}
