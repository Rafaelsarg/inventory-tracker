using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSolution.Models
{
    public class ProductType
    {
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        // Navigation property
        public ICollection<Product> Products { get; set; }
    }
}