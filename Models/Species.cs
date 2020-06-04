using System.ComponentModel.DataAnnotations;

namespace OrchardManagement.Models
{
    public class Species
    {
        public int ID { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Description { get; set; }
    }
}
