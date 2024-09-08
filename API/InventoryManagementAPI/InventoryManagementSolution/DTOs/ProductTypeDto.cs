using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSolution.DTOs;

public class ProductTypeDto
{
    [Required]
    public int inventoryid { get; set; }

    [Required]
    [StringLength(255)]
    public string name { get; set; }
    
}