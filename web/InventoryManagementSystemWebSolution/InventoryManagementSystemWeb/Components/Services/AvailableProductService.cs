using InventoryManagementSystemWeb.Components.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
namespace InventoryManagementSystemWeb.Components.Services;

public class AvailableProductResponse
{
    [JsonPropertyName("$values")]
    public List<AvailableProductModel> values { get; set; }
}

public interface IAvailableProductService
{
    Task<List<AvailableProductModel>> GetAvailableProductsByInventoryId(int inventoryId);
    Task<List<AvailableProductModel>> GetAvailableProductsByRestockingTransactionId(int restockingTransactionId);
    
    Task<List<AvailableProductModel>> GetAvailableProductsByProductId(int productId); 
    
    Task<List<AvailableProductModel>> AddAvailableProducts(List<AvailableProductModel> availableProducts);
}
    
public class AvailableProductService : IAvailableProductService
{
    private readonly HttpClient _httpClient;

    public AvailableProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<AvailableProductModel>> GetAvailableProductsByInventoryId(int inventoryId)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5248/api/AvailableProducts/ByInventory/{inventoryId}");
        var availableProductResponse = await response.Content.ReadFromJsonAsync<AvailableProductResponse>();
        return availableProductResponse.values;

    }

    public async Task<List<AvailableProductModel>> GetAvailableProductsByRestockingTransactionId(int restockingTransactionId)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5248/api/AvailableProducts/ByRestockingTransaction/{restockingTransactionId}");
        var availableProductResponse = await response.Content.ReadFromJsonAsync<AvailableProductResponse>();
        return availableProductResponse.values;
    }
    
    public async Task<List<AvailableProductModel>> GetAvailableProductsByProductId(int productId)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5248/api/AvailableProducts/ByProduct/{productId}");
        var availableProductResponse = await response.Content.ReadFromJsonAsync<AvailableProductResponse>();
        return availableProductResponse.values;
    }
    
    public async Task<List<AvailableProductModel>> AddAvailableProducts(List<AvailableProductModel> availableProducts)
    {
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5248/api/AvailableProducts", availableProducts);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<AvailableProductModel>>();
        }
        else
        {
            // Handle the error according to your application's needs
            throw new HttpRequestException("Failed to add available products.");
        }
    }

}
