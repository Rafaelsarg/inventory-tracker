using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using InventoryManagementSystemWeb.Components.Models;

namespace InventoryManagementSystemWeb.Components.Services
{
    public class ProductResponse
    {

        [JsonPropertyName("$values")] 
        public List<ProductModel> values { get; set; }
    }

    public interface IProductService
    {
        Task<List<ProductModel>> GetProductsByProductTypeId(int productTypeId);

        Task<ProductModel> AddProduct(ProductModel product);
    }

    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly List<ProductModel> _products;


        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductModel>> GetProductsByProductTypeId(int productTypeId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5248/api/Product/{productTypeId}");
            var productResponse = await response.Content.ReadFromJsonAsync<ProductResponse>();
            return productResponse.values;
        }


        public async Task<ProductModel> AddProduct(ProductModel product)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5248/api/Product", product);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductModel>();
            }
            else
            {
                // Read the error content from the response (if any)
                var errorContent = await response.Content.ReadAsStringAsync();
            
                // Log the error (you can use a logging framework here)
                Console.WriteLine($"Error creating product: {response.StatusCode} - {errorContent}");

                return null;
            }
        }
    }
}
