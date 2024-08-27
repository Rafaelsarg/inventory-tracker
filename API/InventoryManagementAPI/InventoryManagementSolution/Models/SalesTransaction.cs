using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSolution.Models
{
    public class SalesTransaction
    {
        public int id { get; set; }

        [Required]
        public int customerid { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal totalcost { get; set; }

        public DateTime restockingdate { get; set; }

        // Navigation property
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public ICollection<SoldProductInstance> SoldProductInstances { get; set; }
    }
}