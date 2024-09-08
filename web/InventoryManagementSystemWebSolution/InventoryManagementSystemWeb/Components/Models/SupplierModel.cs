using System.ComponentModel.DataAnnotations;
namespace InventoryManagementSystemWeb.Components.Models;

public class SupplierModel
{
    public int id { get; set; }

    [Required]
    public int inventoryid { get; set; }
        
    [Required]
    [StringLength(255)]
    public string name { get; set; }

    [StringLength(255)]
    public string contactinfo { get; set; }
}