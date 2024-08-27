using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSolution.Models
{
    public class AvailableProductInstance
    {
        public int id { get; set; }

        [Required]
        public int productid { get; set; }

        [Required]
        public int inventoryid { get; set; }

        [Required]
        public int restockingtransactionid { get; set; }

        [StringLength(255)]
        public string sku { get; set; }

        // Navigation properties
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("InventoryId")]
        public Inventory Inventory { get; set; }

        [ForeignKey("RestockingTransactionId")]
        public RestockingTransaction RestockingTransaction { get; set; }
    }
}
