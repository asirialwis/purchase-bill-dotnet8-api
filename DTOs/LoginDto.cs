using System.ComponentModel.DataAnnotations;

namespace PurchaseBillAPI.DTOs;

public class LoginDto 
{
    [Required(ErrorMessage = "Username is required.")]
    [EmailAddress(ErrorMessage = "Username must be a valid email address.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}