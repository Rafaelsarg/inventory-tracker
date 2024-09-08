using InventoryManagementSystemWeb.Components.Models;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
namespace InventoryManagementSystemWeb.Components.Services;

public class RestockingTransactionResponse
{

    [JsonPropertyName("$values")] 
    public List<RestockingTransactionModel> values { get; set; }
}

public interface IRestockingTransactionService
{

    Task<List<RestockingTransactionModel>> GetRestockingTransactionsBySupplierId(int supplierId);
    Task<RestockingTransactionModel> AddRestockingTransaction(RestockingTransactionModel restockingTransactionDto);
    Task<bool> DeleteRestockingTransaction(int transactionId);
}

public class RestockingTransactionService : IRestockingTransactionService
{
    private readonly HttpClient _httpClient;

    public RestockingTransactionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<RestockingTransactionModel>> GetRestockingTransactionsBySupplierId(int supplierId)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5248/api/RestockingTransaction/bySupplier/{supplierId}");
        var supplierResponse = await response.Content.ReadFromJsonAsync<RestockingTransactionResponse>();
        return supplierResponse.values;
    }

    public async Task<RestockingTransactionModel> AddRestockingTransaction(RestockingTransactionModel restockingTransactionDto)
    {
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5248/api/RestockingTransaction", restockingTransactionDto);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Done");
            return await response.Content.ReadFromJsonAsync<RestockingTransactionModel>();
        }
        else
        {
            // Capture the status code and response content for better error handling
            var statusCode = response.StatusCode;
            var errorContent = await response.Content.ReadAsStringAsync();

            // Construct a more informative error message
            var errorMessage = $"Failed to add available products. Status Code: {(int)statusCode} ({statusCode}), Error: {errorContent}";

            // Throw the exception with the detailed error message
            throw new HttpRequestException(errorMessage);
        }
        
        
    }
    
    public async Task<bool> DeleteRestockingTransaction(int transactionId)
    {
        var response = await _httpClient.DeleteAsync($"http://localhost:5248/api/RestockingTransaction/{transactionId}");

        var responseContent = await response.Content.ReadAsStringAsync();
        
        Console.WriteLine(responseContent);
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