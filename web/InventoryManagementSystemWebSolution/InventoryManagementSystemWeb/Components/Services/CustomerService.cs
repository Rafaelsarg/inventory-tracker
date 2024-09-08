using InventoryManagementSystemWeb.Components.Models;
using System.Text.Json.Serialization;
namespace InventoryManagementSystemWeb.Components.Services;

public class CustomerResponse
{

    [JsonPropertyName("$values")] 
    public List<CustomerModel> values { get; set; }
}

public interface ICustomerService
{
    Task<List<CustomerModel>> GetCustomersByInventoryId(int inventoryId);
    Task<CustomerModel> AddCustomer(CustomerModel customerDto);
    Task<bool> DeleteCustomer(int customerId);
}

public class CustomerService : ICustomerService
{
    private readonly HttpClient _httpClient;

    public CustomerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CustomerModel>> GetCustomersByInventoryId(int inventoryId)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5248/api/Customer/byInventory/{inventoryId}");
        var customerResponse = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        return customerResponse.values;
    }

    public async Task<CustomerModel> AddCustomer(CustomerModel customerDto)
    {
        
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5248/api/Customer", customerDto);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CustomerModel>();
        }
        else
        {
            // Handle error (e.g., throw an exception, log the error, etc.)
            throw new HttpRequestException("Error adding customers");
        }
    }
    
    public async Task<bool> DeleteCustomer(int customerId)
    {
        // Sending a DELETE request to the API
        var response = await _httpClient.DeleteAsync($"http://localhost:5248/api/Customer/{customerId}");
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