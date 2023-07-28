using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class KullaniciGirisModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string? KullaniciAdi { get; set; }

        [Required]
        [StringLength(10)]
        public string? Sifre { get; set; }
    }
}
