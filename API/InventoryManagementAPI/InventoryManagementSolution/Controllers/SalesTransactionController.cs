using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSolution.Data;
using InventoryManagementSolution.Models;
using InventoryManagementSolution.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace InventoryManagementSolution.Controllers;


[Route("api/[controller]")]
[ApiController]
public class SalesTransactionController : ControllerBase
{
    private readonly MyDbContext _context;

    public SalesTransactionController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SalesTransaction>> GetSalesTransactionById(int id)
    {
        var restockingTransaction = await _context.restockingtransaction.FindAsync(id);

        if (restockingTransaction == null)
        {
            return NotFound();
        }

        return Ok(restockingTransaction);
    }

    [HttpGet("byCustomer/{customerId}")]
    public async Task<ActionResult<IEnumerable<SalesTransaction>>> GetSalesTransactionsBySupplierId(int customerId)
    {
        var salesTransactions = await _context.salestransaction
            .Include(rt => rt.SoldProductInstances)
            .Where(rt => rt.customerid == customerId)
            .ToListAsync();


        return Ok(salesTransactions);
    }

    // POST: api/SalesTransaction
    [HttpPost]
    public async Task<ActionResult<SalesTransaction>> PostRestockingTransaction(SalesTransactionDto salesTransactionDto)
    {
        // Check if the Customer exists
        var customer = await _context.customer.FindAsync(salesTransactionDto.customerid);
        if (customer == null)
        {
            return BadRequest(new { Message = "Customer not found." });
        }

        List<SoldProductInstance> soldProductInstances = new List<SoldProductInstance>();
        
        // Start transaction to ensure atomic operation
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                foreach (var sku in salesTransactionDto.skus)
                {
                    // Find the AvailableProductInstance by SKU
                    var availableProductInstance = await _context.availableproductinstance
                        .FirstOrDefaultAsync(api => api.sku == sku);

                    if (availableProductInstance == null)
                    {
                        return BadRequest(new { Message = $"Product with SKU {sku} not found." });
                    }

                    // Create the SoldProductInstance
                    SoldProductInstance soldProductInstance = new SoldProductInstance()
                    {
                        sku = sku,
                    };

                    soldProductInstances.Add(soldProductInstance);

                    // Remove the AvailableProductInstance from the inventory
                    _context.availableproductinstance.Remove(availableProductInstance);
                }

                // Create the SalesTransaction entity
                var salesTransaction = new SalesTransaction()
                {
                    customerid = salesTransactionDto.customerid,
                    totalcost = salesTransactionDto.totalcost,
                    restockingdate = DateTime.UtcNow,
                    SoldProductInstances = soldProductInstances,
                };

                // Save the SalesTransaction and SoldProductInstances
                _context.salestransaction.Add(salesTransaction);
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetSalesTransactionById), new { id = salesTransaction.id },
                    salesTransaction);
            }
            catch (Exception ex)
            {
                // Rollback transaction in case of an error
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesTransaction(int id)
    {
        // Find the sales transaction by ID
        var salesTransaction = await _context.salestransaction
            .Include(rt => rt.SoldProductInstances) 
            .FirstOrDefaultAsync(rt => rt.id == id);
            
        if (salesTransaction == null)
        {
            return NotFound(new { Message = "Restocking transaction not found." });
        }

        if (salesTransaction.SoldProductInstances.Any())
        {
            _context.soldproductinstance.RemoveRange(salesTransaction.SoldProductInstances);
        }

        _context.salestransaction.Remove(salesTransaction);
        await _context.SaveChangesAsync();

        return NoContent(); // Return 204 No Content to indicate success
    }
}
        