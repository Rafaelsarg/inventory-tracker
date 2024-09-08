using System.ComponentModel.DataAnnotations;
namespace InventoryManagementSystemWeb.Components.Models;

public class ProductModel
{
    public int id { get; set; }
    
    [Required]
    public int producttypeid { get; set; }

    [Required]
    [StringLength(255)]
    public string name { get; set; }

    public DateTime creation { get; set; }
}