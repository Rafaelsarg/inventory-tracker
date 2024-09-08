using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InventoryManagementSolution.DTOs;

public class SalesTransactionDto
{
    [Required]
    public int customerid { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal totalcost { get; set; }

    public DateTime restockingdate { get; set; }
    
    public List<string> skus { get; set; }
}