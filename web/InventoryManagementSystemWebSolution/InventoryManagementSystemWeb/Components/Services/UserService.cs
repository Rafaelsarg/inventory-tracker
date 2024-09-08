using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System;
using InventoryManagementSystemWeb.Components.Models;
namespace InventoryManagementSystemWeb.Components.Services
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreationTime { get; set; } // Ensure this property has get/set accessors
    }

    public interface IUserService
    {
        User User { get; set; }
        Task<ApiResponse<User>> GetUserInfoByEmailAsync(string email);
        Task<ApiResponse<User>> GetUserInfoByIDAsync(int id);
        Task<ApiResponse<bool>> LoginAsync(LoginModel loginModel);
        Task<ApiResponse<bool>> CreateUserAsync(NewUserModel newUserModel);
    }

    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        public User User { get; set; }

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            User = new User();
        }

        public async Task<ApiResponse<User>> GetUserInfoByEmailAsync(string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:5248/api/Users/email/{email}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<User>();
                    User = user;
                    return new ApiResponse<User>(true, user);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<User>(false, null, error);
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<User>(false, null, ex.Message);
            }
        }

        public async Task<ApiResponse<User>> GetUserInfoByIDAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:5248/api/Users/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<User>();
                    User = user;
                    return new ApiResponse<User>(true, user);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<User>(false, null, error);
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<User>(false, null, ex.Message);
            }
        }
        
        public async Task<ApiResponse<bool>> LoginAsync(LoginModel loginModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://localhost:5248/api/Users/login", loginModel);
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
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, false, ex.Message);
            }
        }

        
        public async Task<ApiResponse<bool>> CreateUserAsync(NewUserModel newUserModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://localhost:5248/api/Users/register", newUserModel);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<bool>(true, true);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<bool>(false, false, error);
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, false, ex.Message);
            }
        }
    }

    public class ApiResponse<T>
    {
        public bool IsSuccess { get; }
        public T Data { get; }
        public string ErrorMessage { get; }

        public ApiResponse(bool isSuccess, T data, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
        }
    }
}
