using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSolution.Models
{
    public class ProductType
    {
        public int id { get; set; }
        
        [Required]
        public int inventoryid { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        // Navigation property
        public ICollection<Product> Products { get; set; }
        
        [ForeignKey("inventoryid")]
        public Inventory Inventory { get; set; }
        
    }
}