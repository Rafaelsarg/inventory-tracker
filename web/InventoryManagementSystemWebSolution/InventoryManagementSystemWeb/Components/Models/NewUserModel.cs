using System.ComponentModel.DataAnnotations;
namespace InventoryManagementSystemWeb.Components.Models;

public class NewUserModel
{
    [Required(ErrorMessage = "First Name is required.")]
    public string firstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last Name is required.")]
    public string lastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email Address is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string emailAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string password { get; set; } = string.Empty;
}