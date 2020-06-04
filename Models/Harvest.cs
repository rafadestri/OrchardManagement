using System;
using System.ComponentModel.DataAnnotations;

namespace OrchardManagement.Models
{
    public class Harvest
    {
        public int ID { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Information { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Display(Name = "Gross Weight")]
        public decimal GrossWeight { get; set; }
        [Display(Name = "Tree")]
        public int TreeID { get; set; }
        public Tree Trees { get; set; }

    }
}
