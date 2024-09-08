using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystemWeb.Components.Models;

public class AvailableProductModel
{
    [Required]
    public int productid { get; set; }

    [Required]
    public int inventoryid { get; set; }

    public int restockingtransactionid { get; set; }

    [StringLength(255)]
    public string sku { get; set; }
}