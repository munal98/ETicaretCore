using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace Entities.Entities
{
    public class Kullanici : RecordBase
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string? KullaniciAdi { get; set; }

        [Required]
        [StringLength(10)]
        public string? Sifre { get; set; }

        public bool Active { get; set; }

        public int KullaniciDetayiId { get; set; }
        public KullaniciDetayi? KullaniciDetayi { get; set; }

        public int RolId { get; set; }
        public Rol? Rol { get; set; }
    }
}
