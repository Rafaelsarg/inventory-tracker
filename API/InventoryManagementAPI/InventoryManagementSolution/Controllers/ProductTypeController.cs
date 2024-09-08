using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSolution.Data;
using InventoryManagementSolution.Models;
using InventoryManagementSolution.DTOs;

namespace InventoryManagementSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ProductTypeController(MyDbContext context)
        {
            _context = context;
        }
        
        // GET: api/ProductType/InventoryID
        [HttpGet("{inventoryId}")]
        public async Task<ActionResult<ProductType>> GetProductTypeByInventoryId(int inventoryId)
        {
            // Fetch the product types associated with the given Inventory ID
            var productTypes = await _context.producttype
                .Where(pt => pt.inventoryid == inventoryId)
                .ToListAsync();
            
            return Ok(productTypes);
            
        }
        
        // POST: api/ProductType
        [HttpPost]
        public async Task<ActionResult<ProductType>> PostProductType(ProductTypeDto productType)
        {
            // Check if the ProductType name is unique within the same inventory
            var existingProductType = await _context.producttype
                .Where(pt => pt.inventoryid == productType.inventoryid && pt.name == productType.name)
                .FirstOrDefaultAsync();

            if (existingProductType != null)
            {
                return Conflict(new { Message = "A ProductType with the same name already exists in this inventory." });
            }
            
            var _producttype = new ProductType()
            {
                inventoryid = productType.inventoryid,
                name = productType.name,
            };
            
            // Add the new ProductType to the context
            _context.producttype.Add(_producttype);
            await _context.SaveChangesAsync();

            // Return the created ProductType along with the route to access it
            return CreatedAtAction(nameof(GetProductTypeByInventoryId), new { inventoryId = _producttype.inventoryid },
                productType);
        }
    }
}