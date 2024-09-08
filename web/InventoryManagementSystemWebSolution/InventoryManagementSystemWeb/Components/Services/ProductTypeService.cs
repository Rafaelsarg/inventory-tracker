using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using InventoryManagementSystemWeb.Components.Models;

namespace InventoryManagementSystemWeb.Components.Services
{
    public interface IProductTypeService
    {
        Task<List<ProductTypeModel>> GetProductTypesByInventoryId(int inventoryId);
        Task<ProductTypeModel> AddProductType(ProductTypeModel productType);

    }
    public class ProductTypeResponse
    {

        [JsonPropertyName("$values")] 
        public List<ProductTypeModel> values { get; set; }
    }

    public class ProductTypeService : IProductTypeService
    {
        private readonly HttpClient _httpClient;

        public ProductTypeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductTypeModel>> GetProductTypesByInventoryId(int inventoryId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5248/api/ProductType/{inventoryId}");
            var productTypeResponse = await response.Content.ReadFromJsonAsync<ProductTypeResponse>();
            return productTypeResponse.values;
        }
        
        public async Task<ProductTypeModel> AddProductType(ProductTypeModel productType)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5248/api/ProductType", productType);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductTypeModel>();
            }
            else
            {
                // Read the error content from the response (if any)
                var errorContent = await response.Content.ReadAsStringAsync();
            
                // Log the error (you can use a logging framework here)
                Console.WriteLine($"Error creating product type: {response.StatusCode} - {errorContent}");

                return null;
            }
        }

    }
}