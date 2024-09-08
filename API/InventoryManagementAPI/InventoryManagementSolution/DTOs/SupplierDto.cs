using System.ComponentModel.DataAnnotations;
namespace InventoryManagementSolution.DTOs;

public class SupplierDto
{
    [Required]
    public int inventoryid { get; set; }
        
    [Required]
    [StringLength(255)]
    public string name { get; set; }

    [StringLength(255)]
    public string contactinfo { get; set; }
}