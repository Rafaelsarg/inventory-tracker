using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSolution.Data;
using InventoryManagementSolution.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailableProductsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public AvailableProductsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/AvailableProductInstance/ByInventory/1
        [HttpGet("ByInventory/{inventoryId}")]
        public async Task<ActionResult<IEnumerable<AvailableProductInstance>>> GetAvailableProductsByInventoryId(int inventoryId)
        {
            var availableProducts = await _context.availableproductinstance
                .Where(api => api.inventoryid == inventoryId)
                .ToListAsync();

            return Ok(availableProducts);
        }

        // GET: api/AvailableProductInstance/ByRestockingTransaction/2
        [HttpGet("ByRestockingTransaction/{restockingTransactionId}")]
        public async Task<ActionResult<IEnumerable<AvailableProductInstance>>> GetAvailableProductsByRestockingTransactionId(int restockingTransactionId)
        {
            var availableProducts = await _context.availableproductinstance
                .Where(api => api.restockingtransactionid == restockingTransactionId)
                .ToListAsync();

            return Ok(availableProducts);
        }
        
        // GET: api/AvailableProductInstance/ByProduct/3
        [HttpGet("ByProduct/{productId}")]
        public async Task<ActionResult<IEnumerable<AvailableProductInstance>>> GetAvailableProductsByProductId(int productId)
        {
            var availableProducts = await _context.availableproductinstance
                .Where(api => api.productid == productId)
                .ToListAsync();

            return Ok(availableProducts);
        }
        
        // POST: api/AvailableProductInstance
        [HttpPost("")]
        public async Task<ActionResult> AddAvailableProducts(List<AvailableProductInstance> availableProducts)
        {
            if (availableProducts == null || availableProducts.Count == 0)
            {
                return BadRequest("No available products provided.");
            }

            foreach (var product in availableProducts)
            {
                _context.availableproductinstance.Add(product);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAvailableProductsByInventoryId), new { inventoryId = availableProducts.First().inventoryid }, availableProducts);
        }
    }
    
}