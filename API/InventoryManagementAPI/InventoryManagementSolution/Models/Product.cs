using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSolution.Models
{
    public class Product
    {
        public int id { get; set; }

        [Required]
        public int producttypeid { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public DateTime creation { get; set; }

        // Navigation property
        [ForeignKey("ProductTypeId")]
        public ProductType ProductType { get; set; }
        
        public ICollection<AvailableProductInstance> AvailableProductInstances { get; set; }
    }
}