using System.Text;
using BookShopServer;
using BookShopServer.DTOs;
using BookShopServer.Entities;
using BookShopServer.Repositories;
using BookShopServer.Repositories.ItemRepositories;
using BookShopServer.Repositories.ItemRepositories.ItemCondition;
using BookShopServer.Repositories.ItemRepositories.ItemType;
using BookShopServer.Repositories.OrderRepositories;
using BookShopServer.Repositories.UserRepositories;
using BookShopServer.Services;
using BookShopServer.Services.ItemServices;
using BookShopServer.Services.ItemServices.ItemCondition;
using BookShopServer.Services.ItemServices.ItemType;
using BookShopServer.Services.OrderServices;
using BookShopServer.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// Create a new web application builder instance.
var builder = WebApplication.CreateBuilder(args);

// Add services to the dependency injection container.

// Registers controllers for handling HTTP requests and Swagger for API documentation.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

var key = builder.Configuration["Jwt:Key"];
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
        };
    });

// Register all repositories and services for dependency injection.
// Using AddScoped means a new instance is created once per client request.

// Age Category Services and Repositories
builder.Services.AddScoped<IAgeCategoryRepository, AgeCategoryRepository>();
builder.Services.AddScoped<IAgeCategoryService, AgeCategoryService>();

// Item-related Services and Repositories (general and specific types/conditions)
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IUsedItemRepository, UsedItemRepository>();
builder.Services.AddScoped<INewItemRepository, NewItemRepository>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<INewItemService, NewItemService>();
builder.Services.AddScoped<IUsedItemService, UsedItemService>();

// Book, Magazine, Newspaper specific Repositories and Services
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IMagazineRepository, MagazineRepository>();
builder.Services.AddScoped<INewspaperRepository, NewspaperRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMagazineService, MagazineService>();
builder.Services.AddScoped<INewspaperService, NewspaperService>();

// Genre, Author, Publisher Repositories and Services
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();


// Order-related Services and Repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// User-related Services and Repositories (general user, customer, employee)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Review Services and Repositories
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();

// Authentication and Authorization Services
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();


// Configure JSON serialization options.
// This specifically adds a custom converter to exclude milliseconds from DateTime serialization,
// ensuring a cleaner JSON output for dates.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateTimeConverterWithoutMilliseconds());
    });

// Register a background hosted service.
// CartExpirationWorker will run periodically to clean up expired shopping carts in the background.
builder.Services.AddHostedService<CartExpirationWorker>();

// Configure Entity Framework Core with SQLite.
// Specifies that the application will use SQLite and defines the connection string
// to the 'bookshop.db' file located in the 'Database' folder.
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlite("Data Source=Database/bookshop.db"));

// Build the web application.
var app = builder.Build();

// Configure the HTTP request pipeline.

// Ensure the database is created or migrated at application startup.
// A service scope is created to resolve the DbContext, and then Database.EnsureCreated()
// attempts to create the database if it doesn't already exist.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Context>();
    try
    {
        db.Database.EnsureCreated(); // Creates the database if it doesn't exist
        if (!db.Publishers.Any())
        {
            db.Publishers.Add(new Publisher { Name = "Penguin Random House", Address = new Address { Street = "Broadway", HouseNumber = 1745, City = "New York", PostalCode = "10019", Country = "USA" }, Email = "info@penguinrandomhouse.com", PhoneNumber = "212782900" });
            db.Publishers.Add(new Publisher { Name = "HarperCollins Publishers", Address = new Address { Street = "East 53rd Street", HouseNumber = 195, City = "New York", PostalCode = "10022", Country = "USA" }, Email = "contact@harpercollins.com", PhoneNumber = "212207700" });
            db.Publishers.Add(new Publisher { Name = "The New York Times", Address = new Address { Street = "8th Avenue", HouseNumber = 620, City = "New York", PostalCode = "10018", Country = "USA" }, Email = "nytco@nytimes.com", PhoneNumber = "212556123" });
            db.Publishers.Add(new Publisher { Name = "Vogue", Address = new Address { Street = "One World Trade Center", HouseNumber = 1, City = "New York", PostalCode = "10007", Country = "USA" }, Email = "contact@condenast.com", PhoneNumber = "212286286" });
            db.Publishers.Add(new Publisher { Name = "Springer Nature", Address = new Address { Street = "Heidelberg Platz", HouseNumber = 1, City = "Berlin", PostalCode = "14197", Country = "Germany" }, Email = "info@springernature.com", PhoneNumber = "493082780" });
            db.Publishers.Add(new Publisher { Name = "Hearst Communications", Address = new Address { Street = "Eighth Avenue", HouseNumber = 300, City = "New York", PostalCode = "10019", Country = "USA" }, Email = "info@hearst.com", PhoneNumber = "212649200" });
            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        // Log any errors that occur during database creation.
        Console.WriteLine("DB creation failed: " + ex.Message);
    }
}

// Enable Swagger UI only in development environment.
// Swagger provides an interactive API documentation and testing interface.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enables the Swagger middleware
    app.UseSwaggerUI(); // Enables the Swagger UI (web page)
}

// Load any persisted static configuration, such as the minimum salary for employees.
MinimumSalaryPersistence.Load();

app.UseAuthentication();
app.UseAuthorization();

// Configure HTTP redirection for HTTPS.
app.UseHttpsRedirection();

// Map controller endpoints to handle incoming HTTP requests.
app.MapControllers();

// Run the application, starting the web server.
app.Run();