using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSolution.Data;
using InventoryManagementSolution.Models;
using InventoryManagementSolution.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestockingTransactionController : ControllerBase
    {
        private readonly MyDbContext _context;

        public RestockingTransactionController(MyDbContext context)
        {
            _context = context;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<RestockingTransaction>> GetRestockingTransactionById(int id)
        {
            var restockingTransaction = await _context.restockingtransaction.FindAsync(id);
    
            if (restockingTransaction == null)
            {
                return NotFound();
            }

            return Ok(restockingTransaction);
        }
        
        // GET: api/RestockingTransaction/bySupplier/5
        [HttpGet("bySupplier/{supplierId}")]
        public async Task<ActionResult<IEnumerable<RestockingTransaction>>> GetRestockingTransactionsBySupplierId(int supplierId)
        {
            var restockingTransactions = await _context.restockingtransaction
                .Include(rt => rt.AvailableProductInstances)
                .Where(rt => rt.supplierid == supplierId)
                .ToListAsync();
            

            return Ok(restockingTransactions);
        }

        // POST: api/RestockingTransaction
        [HttpPost]
        public async Task<ActionResult<RestockingTransaction>> PostRestockingTransaction(RestockingTransactionDto restockingTransactionDto)
        {
            Console.WriteLine(restockingTransactionDto.supplierid);
            // Check if the Supplier exists
            var supplier = await _context.supplier.FindAsync(restockingTransactionDto.supplierid);
            if (supplier == null)
            {
                return BadRequest(new { Message = "Supplier not found." });
            }

            List<AvailableProductInstance> availableProductInstances = new List<AvailableProductInstance>();
            foreach (var sku in restockingTransactionDto.skus)
            {
                AvailableProductInstance availableProductInstance = new AvailableProductInstance()
                {
                    productid = restockingTransactionDto.productid,
                    sku = sku,
                    inventoryid = restockingTransactionDto.inventoryid
                };
                
                availableProductInstances.Add(availableProductInstance);

            }
            
            // Create the RestockingTransaction entity
            var restockingTransaction = new RestockingTransaction
            {
                supplierid = restockingTransactionDto.supplierid,
                totalcost = restockingTransactionDto.totalcost,
                restockingdate = DateTime.UtcNow,
                
                AvailableProductInstances = availableProductInstances,
            };
            
            // Save the RestockingTransaction and AvailableProductInstances
            _context.restockingtransaction.Add(restockingTransaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRestockingTransactionById), new { id = restockingTransaction.id }, restockingTransaction);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestockingTransaction(int id)
        {
            // Find the restocking transaction by ID
            var restockingTransaction = await _context.restockingtransaction
                .Include(rt => rt.AvailableProductInstances) // Include related AvailableProductInstances
                .FirstOrDefaultAsync(rt => rt.id == id);
            
            if (restockingTransaction == null)
            {
                return NotFound(new { Message = "Restocking transaction not found." });
            }

            // Remove associated available product instances
            if (restockingTransaction.AvailableProductInstances.Any())
            {
                _context.availableproductinstance.RemoveRange(restockingTransaction.AvailableProductInstances);
            }

            // Remove the restocking transaction
            _context.restockingtransaction.Remove(restockingTransaction);
            await _context.SaveChangesAsync();

            return NoContent(); // Return 204 No Content to indicate success
        }
        
    }
}
