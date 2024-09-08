using System.ComponentModel.DataAnnotations;
namespace InventoryManagementSolution.DTOs;

public class ProductDto
{
    [Required]
    public int producttypeid { get; set; }

    [Required]
    [StringLength(255)]
    public string name { get; set; }

    public DateTime creation { get; set; }
}