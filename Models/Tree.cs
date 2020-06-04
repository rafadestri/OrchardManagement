using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrchardManagement.Models
{
    public class Tree
    {
        public int ID { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Description { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public decimal Age { get; set; }
        [Display(Name = "Species")]
        public int SpeciesID { get; set; }
        public Species Species { get; set; }

        private ICollection<TreeGroupTree> _treeGroupTrees;
        public ICollection<TreeGroupTree> TreeGroupTrees
        {
            get
            {
                return _treeGroupTrees ?? (_treeGroupTrees = new List<TreeGroupTree>());
            }
            set
            {
                _treeGroupTrees = value;
            }
        }
    }
}
