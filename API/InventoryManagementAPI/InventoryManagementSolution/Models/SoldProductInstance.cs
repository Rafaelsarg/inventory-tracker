using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSolution.Models
{
    public class SoldProductInstance
    {
        public int id { get; set; }

        [Required]
        public int salestransactionid { get; set; }

        [StringLength(255)]
        public string sku { get; set; }

        // Navigation property
        [ForeignKey("SalesTransactionId")] public SalesTransaction SalesTransaction { get; set; }
    }
}
