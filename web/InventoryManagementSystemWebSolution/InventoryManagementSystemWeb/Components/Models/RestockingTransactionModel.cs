using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystemWeb.Components.Models;

public class RestockingTransactionModel
{
    public int id { get; set; }
    
    [Required]
    public int supplierid { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal totalcost { get; set; }

    public DateTime restockingdate { get; set; }

    [Required]
    public int productid { get; set; }

    [Required]
    public int inventoryid { get; set; }
    
    public List<string> skus { get; set; }

}