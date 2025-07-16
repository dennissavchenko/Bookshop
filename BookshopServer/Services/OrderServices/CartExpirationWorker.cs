namespace BookShopServer.Services.OrderServices;

/// <summary>
/// A background service that periodically removes expired shopping carts from the system.
/// This worker runs once every 24 hours to clean up old, uncompleted carts.
/// </summary>
public class CartExpirationWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary>
    /// Initializes a new instance of the <c>CartExpirationWorker</c> class.
    /// </summary>
    /// <param name="scopeFactory">The factory used to create service scopes for dependency injection.</param>
    public CartExpirationWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// Executes the background service's logic asynchronously.
    /// This method continuously checks for and removes expired carts at a defined interval.
    /// </summary>
    /// <param name="stoppingToken">A <c>CancellationToken</c> that signals when the service should stop.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Loop until the service is requested to stop
        while (!stoppingToken.IsCancellationRequested)
        {
            // Create a new service scope to resolve scoped services like ICartService
            using (var scope = _scopeFactory.CreateScope())
            {
                // Retrieve the ICartService from the current scope
                var cartService = scope.ServiceProvider.GetRequiredService<ICartService>();
                
                // Call the method to remove expired carts
                await cartService.RemoveExpiredCartsAsync();
                
                // Log the activity to the console
                Console.WriteLine("Expired carts removed at: " + DateTime.Now);
            }

            // Wait for 24 hours before the next execution, respecting the stopping token
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}