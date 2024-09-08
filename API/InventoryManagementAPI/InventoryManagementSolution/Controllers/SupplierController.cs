using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSolution.Data;
using InventoryManagementSolution.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryManagementSolution.DTOs;

namespace InventoryManagementSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly MyDbContext _context;

        public SupplierController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Supplier/byInventory/5
        [HttpGet("byInventory/{inventoryId}")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliersByInventoryId(int inventoryId)
        {
            var suppliers = await _context.supplier
                .Where(s => s.inventoryid == inventoryId)
                .ToListAsync();

            return Ok(suppliers);
        }

        // POST: api/Supplier/byInventory/5
        [HttpPost("")]
        public async Task<ActionResult<Supplier>> PostSupplier(SupplierDto supplier)
        {
            // Check if the Inventory exists
            var inventory = await _context.inventory.FindAsync(supplier.inventoryid);
            if (inventory == null)
            {
                return BadRequest(new { Message = "Inventory not found." });
            }

            Supplier _supplier = new Supplier()
            { 
                inventoryid = supplier.inventoryid,
                name = supplier.name,
                contactinfo = supplier.contactinfo
            };
            
            _context.supplier.Add(_supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSuppliersByInventoryId), new { inventoryID = _supplier.inventoryid }, _supplier);
        }
        
        // DELETE: api/Supplier/5
        [HttpDelete("{supplierId}")]
        public async Task<IActionResult> DeleteSupplier(int supplierId)
        {
            var supplier = await _context.supplier.FindAsync(supplierId);
            if (supplier == null)
            {
                return NotFound(new { Message = "Supplier not found." });
            }

            // Check if there are any restocking transactions linked to the supplier
            var hasRestockingTransactions = await _context.restockingtransaction
                .AnyAsync(rt => rt.supplierid == supplierId);

            if (hasRestockingTransactions)
            {
                return BadRequest(new { Message = "Supplier cannot be deleted because there are associated restocking transactions." });
            }

            _context.supplier.Remove(supplier);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}