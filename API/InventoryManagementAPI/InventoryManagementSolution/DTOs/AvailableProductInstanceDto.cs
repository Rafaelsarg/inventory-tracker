namespace InventoryManagementSolution.DTOs;

public class AvailableProductInstanceDto
{
    public int productid { get; set; }
    public int inventoryid { get; set; }
    public int restockingtransactionid { get; set; }
    public string sku { get; set; }
}