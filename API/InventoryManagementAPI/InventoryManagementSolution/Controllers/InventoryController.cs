using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSolution.Data;
using InventoryManagementSolution.Models;
using InventoryManagementSolution.DTOs;

namespace InventoryManagementSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly MyDbContext _context;

        public InventoryController(MyDbContext context)
        {
            _context = context;
        }
        
        // GET: api/Inventory/{userid}
        [HttpGet("{userid}")]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetUserInventories(int userid)
        {
            var inventory = await _context.inventory
                .Where(u => u.userid == userid)
                .ToListAsync();

            if (inventory == null || !inventory.Any()) 
            {
                return NotFound();
            }
            
            return inventory;
        }
        
        // POST: create inventories
        [HttpPost("create")]
        public async Task<ActionResult<Inventory>> CreateInventory(InventoryDto inventory)
        {
            if (await _context.inventory.AnyAsync(u => u.name == inventory.Name && u.id == inventory.userid))
            {
                return BadRequest("Name already in use.");
            }

            if (await _context.appuser.AnyAsync(ap => ap.id == inventory.userid))
            {
                Console.WriteLine("Exists");
            }
            else
            {
                Console.WriteLine("Does not");
                Console.WriteLine(inventory.userid);
            }
            
            var _inventory = new Inventory()
            {
                userid = inventory.userid,
                name = inventory.Name,
                description = inventory.Description,
                location = inventory.Location,
                creation = DateTime.UtcNow
            };

            // Add the inventory to the database context
            _context.inventory.Add(_inventory);

            // Save the changes asynchronously
            await _context.SaveChangesAsync();

            // Return the created inventory object along with the route to access it
            return CreatedAtAction(nameof(GetUserInventories), new { userid = _inventory.userid, id = _inventory.id }, _inventory);
        }
        
        // DELETE 
        [HttpPost("delete/{inventoryID}")]
        public async Task<ActionResult> DeleteInventory(int inventoryID)
        {
            // Find the inventory by ID
            var inventory = await _context.inventory.FindAsync(inventoryID);

            // Check if the inventory exists
            if (inventory == null)
            {
                // Return a 404 Not Found response if the inventory doesn't exist
                return NotFound("Inventory not found.");
            }

            // Remove the inventory from the database context
            _context.inventory.Remove(inventory);

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Return a 204 No Content response indicating the deletion was successful
            return NoContent();
        }
        
        [HttpPut("update/{inventoryID}")]
        public async Task<ActionResult> UpdateInventory(int inventoryID, InventoryDto inventoryDto)
        {
            // Find the existing inventory in the database
            var existingInventory = await _context.inventory.FindAsync(inventoryID);

            if (existingInventory == null)
            {
                // Return 404 Not Found if the inventory doesn't exist
                return NotFound("Inventory not found.");
            }

            // Update the properties of the existing inventory
            existingInventory.name = inventoryDto.Name;
            existingInventory.description = inventoryDto.Description;
            existingInventory.location = inventoryDto.Location;
            // Update any other necessary fields...

            try
            {
                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Return 204 No Content to indicate success
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflicts if any
                if (!_context.inventory.Any(e => e.id == inventoryID))
                {
                    return NotFound("Inventory not found after concurrency issue.");
                }
                else
                {
                    throw;
                }
            }
        }

    }
}