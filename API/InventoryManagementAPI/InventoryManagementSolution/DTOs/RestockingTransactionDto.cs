using InventoryManagementSolution.Models;

namespace InventoryManagementSolution.DTOs;

public class RestockingTransactionDto
{
    public int supplierid { get; set; }
    public decimal totalcost { get; set; }
    public DateTime restockingdate { get; set; }
    
    public int productid { get; set; }
    public int inventoryid { get; set; }
    public List<string> skus { get; set; }
    
}