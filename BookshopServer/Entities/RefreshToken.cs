namespace BookShopServer.Entities;

public class RefreshToken
{
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public virtual User User { get; set; }
}