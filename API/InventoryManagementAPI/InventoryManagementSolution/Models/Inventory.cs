using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSolution.Models
{
    public class Inventory
    {
        public int id { get; set; }

        [Required]
        public int userid { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public string description { get; set; }

        [StringLength(255)]
        public string location { get; set; }

        public DateTime creation { get; set; }

        // Navigation property
        [ForeignKey("userid")] public AppUser AppUser { get; set; }
        
        public ICollection<AvailableProductInstance> AvailableProductInstances { get; set; }
    }
}