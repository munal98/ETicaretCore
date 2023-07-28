using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace Entities.Entities
{
    public class KullaniciDetayi : RecordBase
    {
        [Required]
        [StringLength(200)]
        public string? EPosta { get; set; }

        [Required]
        [StringLength(1000)]
        public string? Adres { get; set; }

        public Kullanici? Kullanici { get; set; }
    }
}
