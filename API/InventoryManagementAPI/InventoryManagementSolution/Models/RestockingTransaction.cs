using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSolution.Models
{
    public class RestockingTransaction
    {
        public int id { get; set; }

        [Required]
        public int supplierid { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal totalcost { get; set; }

        public DateTime restockingdate { get; set; }

        // Navigation property
        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }

        public ICollection<AvailableProductInstance> AvailableProductInstances { get; set; }
    }
}