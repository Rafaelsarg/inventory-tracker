using InventoryManagementSystemWeb.Components.Models;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security;
using System.Threading.Tasks;
namespace InventoryManagementSystemWeb.Components.Services;

public class SalesTransactionResponse
{

    [JsonPropertyName("$values")] 
    public List<SalesTransactionModel> values { get; set; }
}

public interface ISalesTransactionService
{

    Task<List<SalesTransactionModel>> GetSalesTransactionsBySupplierId(int supplierId);
    Task<SalesTransactionModel> AddSalesTransaction(SalesTransactionModel restockingTransactionDto);

    Task<bool> DeleteTransaction(int transactionId);
}

public class SalesTransactionService : ISalesTransactionService
{
    private readonly HttpClient _httpClient;

    public SalesTransactionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<SalesTransactionModel>> GetSalesTransactionsBySupplierId(int customerId)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5248/api/SalesTransaction/byCustomer/{customerId}");
        var customerResponse = await response.Content.ReadFromJsonAsync<SalesTransactionResponse>();
        return customerResponse.values;
    }

    public async Task<SalesTransactionModel> AddSalesTransaction(SalesTransactionModel salesTransactionDto)
    {
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5248/api/SalesTransaction", salesTransactionDto);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Done");
            return await response.Content.ReadFromJsonAsync<SalesTransactionModel>();
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
    public async Task<bool> DeleteTransaction(int transactionId)
    {
        var response = await _httpClient.DeleteAsync($"http://localhost:5248/api/SalesTransaction/{transactionId}");

        if (response.IsSuccessStatusCode) return true;
        else return false;
    }
}