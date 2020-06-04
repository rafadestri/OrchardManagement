using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrchardManagement.Models
{
    public class TreeGroup
    {
        public int ID { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
