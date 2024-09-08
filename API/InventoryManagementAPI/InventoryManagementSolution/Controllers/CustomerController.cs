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
    public class CustomerController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CustomerController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Customer/byInventory/5
        [HttpGet("byInventory/{inventoryId}")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersByInventoryId(int inventoryId)
        {
            var customers = await _context.customer
                .Where(c => c.inventoryid == inventoryId)
                .ToListAsync();

            return Ok(customers);
        }

        // POST: api/Customer
        [HttpPost("")]
        public async Task<ActionResult<Customer>> PostCustomer(CustomerDto customer)
        {
            // Check if the Inventory exists
            var inventory = await _context.inventory.FindAsync(customer.inventoryid);
            if (inventory == null)
            {
                return BadRequest(new { Message = "Inventory not found." });
            }

            Customer _customer = new Customer()
            { 
                inventoryid = customer.inventoryid,
                name = customer.name,
                contactinfo = customer.contactinfo
            };
            
            _context.customer.Add(_customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomersByInventoryId), new { inventoryId = _customer.inventoryid }, _customer);
        }
        
        // DELETE: api/Customer/5
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomer(int customerId)
        {
            var customer = await _context.customer.FindAsync(customerId);
            if (customer == null)
            {
                return NotFound(new { Message = "Customer not found." });
            }

            // Check if there are any restocking transactions linked to the supplier
            var hasSalesTransactions = await _context.salestransaction
                .AnyAsync(rt => rt.customerid == customerId);

            if (hasSalesTransactions)
            {
                return BadRequest(new { Message = "Customer cannot be deleted because there are associated sales transactions." });
            }

            _context.customer.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}