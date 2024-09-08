using System.ComponentModel.DataAnnotations;
namespace InventoryManagementSystemWeb.Components.Models;

public class InventoryModel
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public int userid { get; set; }
        
    public string Description { get; set; } = string.Empty;
        
    public string Location { get; set; } = string.Empty;
}