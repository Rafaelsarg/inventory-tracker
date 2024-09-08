using InventoryManagementSystemWeb.Components.Models;
using System.Text.Json.Serialization;
namespace InventoryManagementSystemWeb.Components.Services;

public class SupplierResponse
{

    [JsonPropertyName("$values")] 
    public List<SupplierModel> values { get; set; }
}

public interface ISupplierService
{
    Task<List<SupplierModel>> GetSuppliersByInventoryId(int inventoryId);
    Task<SupplierModel> AddSupplier(SupplierModel supplierDto);

    Task<bool> DeleteSupplier(int supplierId);
}

public class SupplierService : ISupplierService
{
    private readonly HttpClient _httpClient;

    public SupplierService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<SupplierModel>> GetSuppliersByInventoryId(int inventoryId)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5248/api/Supplier/byInventory/{inventoryId}");
        var supplierResponse = await response.Content.ReadFromJsonAsync<SupplierResponse>();
        return supplierResponse.values;
    }

    public async Task<SupplierModel> AddSupplier(SupplierModel supplierDto)
    {
        
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5248/api/Supplier", supplierDto);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<SupplierModel>();
        }
        else
        {
            // Handle error (e.g., throw an exception, log the error, etc.)
            throw new HttpRequestException("Error adding supplier");
        }
    }

    public async Task<bool> DeleteSupplier(int supplierId)
    {
        // Sending a DELETE request to the API
        var response = await _httpClient.DeleteAsync($"http://localhost:5248/api/Supplier/{supplierId}");
        // Read the content of the response
        var responseContent = await response.Content.ReadAsStringAsync();

        // Log the full response content
        Console.WriteLine($"Response: {responseContent}");
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}