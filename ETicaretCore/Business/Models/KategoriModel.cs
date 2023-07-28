using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class KategoriModel : RecordBase
    {
        [Required]
        [StringLength(100)]
        [DisplayName("Kategori Adı")]
        public string? Adi { get; set; }

        [StringLength(400)]
        public string? Aciklamasi { get; set; }
    }
}
