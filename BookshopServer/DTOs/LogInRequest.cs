using System.ComponentModel.DataAnnotations;

namespace BookShopServer.DTOs;

public class LogInRequest
{
    [Required]
    public string UsernameOrEmail { get; set; }
    [Required]
    public string Password { get; set; }
}