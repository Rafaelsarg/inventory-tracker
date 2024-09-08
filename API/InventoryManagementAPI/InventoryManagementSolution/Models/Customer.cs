using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSolution.Models
{
    public class Customer
    {
        public int id { get; set; }
        
        [Required]
        public int inventoryid { get; set; }
        
        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [StringLength(255)]
        public string contactinfo { get; set; }

        [ForeignKey("inventoryid")]
        public Inventory Inventory { get; set; }
        // Navigation property
        public ICollection<SalesTransaction> SalesTransactions { get; set; }
    }
}
