using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSolution.Models
{
    public class AppUser
    {
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string firstname { get; set; }

        [Required]
        [StringLength(255)]
        public string lastname { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string emailaddress { get; set; }

        public DateTime lastlogin { get; set; }

        public DateTime creation { get; set; }

        // Navigation property
        public InternalAuthentication InternalAuthentication { get; set; } = new InternalAuthentication();
        public ICollection<Inventory> Inventories { get; set; }
    }
}

