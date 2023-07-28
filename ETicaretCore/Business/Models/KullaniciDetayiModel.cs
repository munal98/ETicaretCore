using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class KullaniciDetayiModel : RecordBase
    {
        [Required]
        [StringLength(200)]
        public string? EPosta { get; set; }

        [Required]
        [StringLength(1000)]
        public string? Adres { get; set; }
    }
}
