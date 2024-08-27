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
            if (await _context.inventory.AnyAsync(u => u.name == inventory.Name))
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

    }
}