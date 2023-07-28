using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class RolModel : RecordBase
    {
        [Required]
        [StringLength(50)]
        public string? Adi { get; set; }
    }
}
