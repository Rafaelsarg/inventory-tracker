using InventoryManagementSystemWeb.Components.Models; 
using System.Text.Json.Serialization;

namespace InventoryManagementSystemWeb.Components.Services;
public class InventoryResponse
{
    
    [JsonPropertyName("$values")]
    public List<Inventory> values { get; set; }
}

public class Inventory
{
    public int id { get; set; }

    public int userid { get; set; }
    
    public string name { get; set; }

    public string description { get; set; }

    public string location { get; set; }

    public DateTime creation { get; set; }
    
    public bool IsDropdownVisible { get; set; } = false;
}

public interface IInventoryService
{
    Task InitializeInventoriesAsync(int id);
    List<Inventory> GetAllInventories();
    Task<ApiResponse<bool>>  CreateInventoryAsync(InventoryModel inventory);
    
    Task<ApiResponse<bool>>  UpdateInventoryAsync(InventoryModel inventory);

    Task<ApiResponse<bool>> DeleteInventoryAsync(int inventoryID);
}

public class InventoryService : IInventoryService
{
    private readonly HttpClient _httpClient;
    private readonly List<Inventory> _inventories;

    public InventoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _inventories = new List<Inventory>();
    }

    public async Task InitializeInventoriesAsync(int userid)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5248/api/Inventory/{userid}");
        if (response.IsSuccessStatusCode)
        {
            var inventoryResponse = await response.Content.ReadFromJsonAsync<InventoryResponse>();
            if (inventoryResponse?.values != null)
            {
                _inventories.Clear();
                _inventories.AddRange(inventoryResponse.values);
            }
        }
    }

    public List<Inventory> GetAllInventories()
    {
        return _inventories;
    }

    public async Task<ApiResponse<bool>> CreateInventoryAsync(InventoryModel inventory)
    {
        var response = await _httpClient.PostAsJsonAsync($"http://localhost:5248/api/Inventory/create", inventory);
        if (response.IsSuccessStatusCode)
        {
            return new ApiResponse<bool>(true, true);
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            return new ApiResponse<bool>(false, false, errorMessage);
        }
    }

    public async Task<ApiResponse<bool>> UpdateInventoryAsync(InventoryModel inventory)
    {
        var response =
            await _httpClient.PutAsJsonAsync($"http://localhost:5248/api/Inventory/update/{inventory.Id}", inventory);
        if (response.IsSuccessStatusCode)
        {
            return new ApiResponse<bool>(true, true);
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            return new ApiResponse<bool>(false, false, errorMessage);
        }
    }

    public async Task<ApiResponse<bool>> DeleteInventoryAsync(int inventoryID)
    {
        var response = await _httpClient.PostAsJsonAsync($"http://localhost:5248/api/Inventory/delete/{inventoryID}", inventoryID);
        if (response.IsSuccessStatusCode)
        {
            var inventoryToDelete = _inventories.FirstOrDefault(i => i.id == inventoryID);
            if (inventoryToDelete != null)
            {
                _inventories.Remove(inventoryToDelete);
            }

            return new ApiResponse<bool>(true, true);
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            return new ApiResponse<bool>(false, false, errorMessage);
        }
    }
}
