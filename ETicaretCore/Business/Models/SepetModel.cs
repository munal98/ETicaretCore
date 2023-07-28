namespace Business.Models
{
    public class SepetModel
    {
        public int UrunId { get; set; }
        public int KullaniciId { get; set; }
        public string? UrunAdi { get; set; }
        public double BirimFiyati { get; set; }
    }
}
