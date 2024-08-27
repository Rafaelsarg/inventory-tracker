using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSolution.Models
{
    public class Supplier
    {
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [StringLength(255)]
        public string contactinfo { get; set; }

        // Navigation property
        public ICollection<RestockingTransaction> RestockingTransactions { get; set; }
    }
}