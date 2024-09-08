using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSolution.Data;
using InventoryManagementSolution.Models;
using InventoryManagementSolution.DTOs;

namespace InventoryManagementSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly MyDbContext _context; 
        
        public ProductController(MyDbContext context)
        {
            _context = context;
        }
        
        // GET: api/
        [HttpGet("{producttypeId}")]
        public async Task<ActionResult<Product>> GetProductByProductTypeId(int producttypeId)
        {
            // Fetch the product types associated with the given Inventory ID
            var product = await _context.product
                .Where(pt => pt.producttypeid == producttypeId)
                .ToListAsync();

            return Ok(product);
            
        }
        
        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<Product>> PostProductType(ProductDto product)
        {
            
            // Check if the ProductType name is unique within the same inventory
            var existingProductType = await _context.product
                .Where(pt => pt.producttypeid == product.producttypeid && pt.name == product.name)
                .FirstOrDefaultAsync();

            if (existingProductType != null)
            {
                return Conflict(new { Message = "A Product with the same name already exists in this inventory." });
            }
            
            var _product = new Product()
            {
                producttypeid = product.producttypeid,
                name = product.name,
                creation = DateTime.UtcNow,
            };
            
            // Add the new ProductType to the context
            _context.product.Add(_product);
            await _context.SaveChangesAsync();

            // Return the created ProductType along with the route to access it
            return CreatedAtAction(nameof(GetProductByProductTypeId), new { producttypeId = _product.producttypeid },
                product);
        }
    }
}