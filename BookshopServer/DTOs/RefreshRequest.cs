namespace BookShopServer.DTOs;

public class RefreshRequest
{
    public int Id { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}