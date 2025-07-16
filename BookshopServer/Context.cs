using System.Text.Json;
using BookShopServer.DTOs;
using BookShopServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopServer;

/// <summary>
/// Represents the database context for the BookShop application.
/// This context maps application entities to database tables and manages data persistence.
/// </summary>
public class Context : DbContext
{
    /// <summary>
    /// Defines DbSets for all entities in the application, enabling querying and saving operations.
    /// </summary>
    public DbSet<AgeCategory> AgeCategories { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Magazine> Magazines { get; set; }
    public DbSet<Newspaper> Newspapers { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<UsedItem> UsedItems { get; set; }
    public DbSet<NewItem> NewItems { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    /// <summary>
    /// Initializes a new instance of the Context class with the provided options.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public Context(DbContextOptions<Context> options) : base(options) { }

    /// <summary>
    /// Configures the database model, defining table mappings, primary keys,
    /// property constraints, relationships, and inheritance strategies for all entities.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // AgeCategory Entity Configuration
        modelBuilder.Entity<AgeCategory>(entity =>
        {
            // Configure table mapping, primary key, and auto-incrementing ID.
            entity.ToTable("AgeCategory");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            // Configure property lengths and nullability.
            entity.Property(x => x.Tag).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(200).IsRequired();
            entity.Property(x => x.MinimumAge).IsRequired();

            // Add SQL CHECK constraints for data validation.
            entity.ToTable(t => t.HasCheckConstraint("CK_AgeCategory_MinimumAge", "[MinimumAge] BETWEEN 0 AND 100"));
            entity.ToTable(t => t.HasCheckConstraint("CK_AgeCategory_Tag", "[Tag] <> ''"));
            entity.ToTable(t => t.HasCheckConstraint("CK_AgeCategory_Description", "[Description] <> ''"));

            // Define the one-to-many relationship with Item.
            entity.HasMany(x => x.Items)
                  .WithOne(x => x.AgeCategory)
                  .HasForeignKey(x => x.AgeCategoryId);
            
            entity.HasData(
                new AgeCategory { Id = 1, Tag = "Infant", Description = "Content suitable for infants (0-3 years old).", MinimumAge = 0 },
                new AgeCategory { Id = 2, Tag = "Child", Description = "Content suitable for young children (4-11 years old).", MinimumAge = 4 },
                new AgeCategory { Id = 3, Tag = "Pre-Teen", Description = "Content suitable for pre-teens (12-15 years old).", MinimumAge = 12 },
                new AgeCategory { Id = 4, Tag = "Teen", Description = "Content suitable for teenagers (16-17 years old).", MinimumAge = 16 },
                new AgeCategory { Id = 5, Tag = "Adult", Description = "Content suitable for adults (18+ years old).", MinimumAge = 18 }
            );
        });

        // Publisher Entity Configuration
        modelBuilder.Entity<Publisher>(entity =>
        {
            // Configure table mapping, primary key, and auto-incrementing ID.
            entity.ToTable("Publisher");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            // Configure property lengths and nullability.
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(100).IsRequired();
            entity.Property(x => x.PhoneNumber).HasMaxLength(9).IsRequired();

            // Configure the Address as an owned entity.
            entity.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(100).IsRequired();
                address.Property(a => a.City).HasMaxLength(50).IsRequired();
                address.Property(a => a.HouseNumber).IsRequired();
                address.Property(a => a.PostalCode).HasMaxLength(20).IsRequired();
                address.Property(a => a.Country).HasMaxLength(50).IsRequired();
            });

            // Define unique indexes for Email and PhoneNumber.
            entity.HasIndex(x => x.Email).IsUnique();
            entity.HasIndex(x => x.PhoneNumber).IsUnique();

            // Add SQL CHECK constraints.
            entity.ToTable(t => t.HasCheckConstraint("CK_Publisher_Name", "[Name] <> ''"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Publisher_Email", "[Email] <> ''"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Publisher_PhoneNumber", "[PhoneNumber] <> ''"));

            // Define the one-to-many relationship with Item.
            entity.HasMany(x => x.Items)
                  .WithOne(x => x.Publisher)
                  .HasForeignKey(x => x.PublisherId);
        });
        
        // Author Entity Configuration
        modelBuilder.Entity<Author>(entity =>
        {
            // Configure table mapping, primary key, and auto-incrementing ID.
            entity.ToTable("Author");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            // Configure property lengths and nullability.
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Surname).HasMaxLength(100).IsRequired();
            entity.Property(x => x.DOB).IsRequired();
            entity.Property(x => x.Pseudonym).HasMaxLength(200);

            // Add SQL CHECK constraints.
            entity.ToTable(t => t.HasCheckConstraint("CK_Author_Name", "[Name] <> ''"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Author_Surname", "[Surname] <> ''"));

            // Define the many-to-many relationship with Book through a join table (BookAuthor).
            entity.HasMany(x => x.Books)
                  .WithMany(x => x.Authors)
                  .UsingEntity(
                      "BookAuthor",
                      r => r.HasOne(typeof(Book)).WithMany().HasForeignKey("BookId"),
                      l => l.HasOne(typeof(Author)).WithMany().HasForeignKey("AuthorId"),
                      j => j.HasKey("BookId", "AuthorId")
                  );
            
            entity.HasData(
                new Author { Id = 1, Name = "Hugh", Surname = "Howey", DOB = new DateTime(1975, 12, 23), Pseudonym = null },
                new Author { Id = 2, Name = "Lisa", Surname = "Genova", DOB = new DateTime(1970, 7, 22), Pseudonym = null },
                new Author { Id = 3, Name = "Marlo", Surname = "Morgan", DOB = new DateTime(1937, 9, 29), Pseudonym = null },
                new Author { Id = 4, Name = "Astrid", Surname = "Davies", DOB = new DateTime(1988, 3, 15), Pseudonym = "A.D. Write" },
                new Author { Id = 5, Name = "Felix", Surname = "Klausen", DOB = new DateTime(1965, 11, 1), Pseudonym = null },
                new Author { Id = 6, Name = "Zara", Surname = "Hassan", DOB = new DateTime(1992, 5, 20), Pseudonym = "Z.H. Writes" }
            );

        });

        // Item (Base Class for Products) Entity Configuration
        modelBuilder.Entity<Item>(entity =>
        {
            // Configure table mapping, primary key, and auto-incrementing ID.
            entity.ToTable("Item");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            // Configure common property lengths and nullability for all item types.
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(300).IsRequired();
            entity.Property(x => x.ImageUrl).HasMaxLength(300).IsRequired();
            entity.Property(x => x.PublishingDate).IsRequired();
            entity.Property(x => x.Language).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Price).IsRequired();
            entity.Property(x => x.AmountInStock).IsRequired();

            // Configure single-table inheritance (TPH) using the 'IsUsed' discriminator column.
            entity.HasDiscriminator<bool>("IsUsed")
                .HasValue<UsedItem>(true) // Map to UsedItem if IsUsed is true
                .HasValue<NewItem>(false); // Map to NewItem if IsUsed is false
            
            // Define the one-to-one relationship with the base Item entity, with cascade delete.
            entity.HasOne(x => x.Book)
                .WithOne(x => x.Item)
                .HasForeignKey<Book>(x => x.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Define the one-to-one relationship with the base Item entity, with cascade delete.
            entity.HasOne(x => x.Magazine)
                .WithOne(x => x.Item)
                .HasForeignKey<Magazine>(x => x.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Define the one-to-one relationship with the base Item entity, with cascade delete.
            entity.HasOne(x => x.Newspaper)
                .WithOne(x => x.Item)
                .HasForeignKey<Newspaper>(x => x.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add shared SQL CHECK constraints for all item types.
            entity.ToTable(t => t.HasCheckConstraint("CK_Item_Name", "[Name] <> ''"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Item_Description", "[Description] <> ''"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Item_Price", "[Price] >= 0"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Item_AmountInStock", "[AmountInStock] >= 0"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Item_ImageUrl", "[ImageUrl] <> ''"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Item_Language", "[Language] <> ''"));
            entity.ToTable(t => t.HasCheckConstraint("CK_UsedItem_Condition", "[Condition] BETWEEN 0 AND 3"));
        });

        // Book Entity Configuration (inherits from Item)
        modelBuilder.Entity<Book>(entity =>
        {
            // Configure table mapping and primary key (which is also a foreign key to Item).
            entity.ToTable("Book");
            entity.HasKey(x => x.ItemId);

            // Configure Book-specific property constraints and enum conversion.
            entity.Property(x => x.NumberOfPages).IsRequired();
            entity.Property(x => x.CoverType).IsRequired().HasConversion<int>();

            // Add SQL CHECK constraints specific to Book properties.
            entity.ToTable(t => t.HasCheckConstraint("CK_Book_NumberOfPages", "[NumberOfPages] >= 1"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Book_CoverType", "[CoverType] BETWEEN 0 AND 2"));

            // Define the many-to-many relationship with Genre through a join table (BookGenre).
            entity.HasMany(x => x.Genres)
                  .WithMany(x => x.Books)
                  .UsingEntity(
                      "BookGenre",
                      r => r.HasOne(typeof(Genre)).WithMany().HasForeignKey("GenreId"),
                      l => l.HasOne(typeof(Book)).WithMany().HasForeignKey("BookId"),
                      j => j.HasKey("BookId", "GenreId")
                  );
        });

        // Magazine Entity Configuration (inherits from Item)
        modelBuilder.Entity<Magazine>(entity =>
        {
            // Configure table mapping and primary key (foreign key to Item).
            entity.ToTable("Magazine");
            entity.HasKey(x => x.ItemId);

            // Configure Magazine-specific property constraint.
            entity.Property(x => x.IsSpecialEdition).IsRequired();
        });

        // Newspaper Entity Configuration (inherits from Item)
        modelBuilder.Entity<Newspaper>(entity =>
        {
            // Configure table mapping and primary key (foreign key to Item).
            entity.ToTable("Newspaper");
            entity.HasKey(x => x.ItemId);

            // Configure Newspaper-specific property constraints.
            entity.Property(x => x.Headline).HasMaxLength(300).IsRequired();

            // Configure JSON conversion for the 'Topics' List<string> property.
            entity.Property(x => x.Topics)
                .IsRequired()
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<List<string>>(v, JsonSerializerOptions.Default) ?? new List<string>()
                );

            // Add SQL CHECK constraint for Headline.
            entity.ToTable(t => t.HasCheckConstraint("CK_Newspaper_Headline", "[Headline] <> ''"));
        });

        // Review Entity Configuration
        modelBuilder.Entity<Review>(entity =>
        {
            // Configure table mapping, primary key, and auto-incrementing ID.
            entity.ToTable("Review");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            // Configure property constraints for Rating, Text, and TimeStamp.
            entity.Property(x => x.Rating).IsRequired();
            entity.Property(x => x.Text).HasMaxLength(500).IsRequired();
            entity.Property(x => x.TimeStamp).IsRequired();

            // Add SQL CHECK constraints for Rating and Text.
            entity.ToTable(t => t.HasCheckConstraint("CK_Review_Rating", "[Rating] BETWEEN 1 AND 5"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Review_Text", "[Text] <> ''"));

            // Define many-to-one relationships with Customer and Item, with cascade delete for Item.
            entity.HasOne(x => x.Customer)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.CustomerId);

            entity.HasOne(x => x.Item)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // OrderItem Entity Configuration
        modelBuilder.Entity<OrderItem>(entity =>
        {
            // Configure the composite primary key.
            entity.ToTable("OrderItem");
            entity.HasKey(x => new { x.OrderId, x.ItemId });

            // Configure property constraint for Quantity.
            entity.Property(x => x.Quantity).IsRequired();

            // Add SQL CHECK constraint for Quantity.
            entity.ToTable(t => t.HasCheckConstraint("CK_OrderItem_Quantity", "[Quantity] > 0"));

            // Define many-to-one relationships with Order and Item, with cascade delete.
            entity.HasOne(x => x.Order)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Item)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Order Entity Configuration
        modelBuilder.Entity<Order>(entity =>
        {
            // Configure table mapping, primary key, and auto-incrementing ID.
            entity.ToTable("Order");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            // Configure property constraints and enum conversion for OrderStatus.
            entity.Property(x => x.OrderStatus).IsRequired().HasConversion<int>();
            entity.Property(x => x.CreatedAt).IsRequired();

            // Add comprehensive SQL CHECK constraints for order status transitions and timestamps.
            entity.ToTable(t => t.HasCheckConstraint("CK_Order_OrderStatus", "[OrderStatus] BETWEEN 0 AND 6"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Order_ConfirmedAt", "[ConfirmedAt] IS NULL OR [ConfirmedAt] >= [CreatedAt]"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Order_PreparationStartedAt", "[PreparationStartedAt] IS NULL OR [PreparationStartedAt] >= [ConfirmedAt]"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Order_ShippedAt", "[ShippedAt] IS NULL OR [ShippedAt] >= [PreparationStartedAt]"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Order_DeliveredAt", "[DeliveredAt] IS NULL OR [DeliveredAt] >= [ShippedAt]"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Order_CancelledAt", "[CancelledAt] IS NULL OR [CancelledAt] >= [CreatedAt]"));

            // Define relationships with Payment (one-to-one) and Customer (many-to-one).
            entity.HasOne(x => x.Payment)
                .WithOne(x => x.Order)
                .HasForeignKey<Payment>(x => x.OrderId);

            entity.HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId);
        });

        // Payment Entity Configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            // Configure table mapping, primary key, and auto-incrementing ID.
            entity.ToTable("Payment");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            // Configure property constraints and enum conversion for PaymentType.
            entity.Property(x => x.Amount).IsRequired();
            entity.Property(x => x.PaymentType).HasConversion<int>().IsRequired();
            entity.Property(x => x.TimeStamp).IsRequired();

            // Add SQL CHECK constraints for Amount and PaymentType.
            entity.ToTable(t => t.HasCheckConstraint("CK_Payment_Amount", "[Amount] >= 0"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Payment_PaymentType", "[PaymentType] BETWEEN 0 AND 3"));
        });

        // User Entity Configuration (Base Class for Customer and Employee)
        modelBuilder.Entity<User>(entity =>
        {
            // Configure table mapping, primary key, and auto-incrementing ID.
            entity.ToTable("User");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            // Configure common property lengths and nullability for all user types.
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Surname).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Username).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Password).IsRequired();

            // Define unique indexes for Email and Username.
            entity.HasIndex(x => x.Email).IsUnique();
            entity.HasIndex(x => x.Username).IsUnique();

            // Add SQL CHECK constraints.
            entity.ToTable(t => t.HasCheckConstraint("CK_User_Name", "[Name] <> ''"));
            entity.ToTable(t => t.HasCheckConstraint("CK_User_Surname", "[Surname] <> ''"));
            entity.ToTable(t => t.HasCheckConstraint("CK_User_Username", "[Username] <> ''"));
            entity.ToTable(t => t.HasCheckConstraint("CK_User_Email", "[Email] <> ''"));

            entity.HasData(new User
            {
                Id = 1,
                Name = "Admin",
                Surname = "Account",
                Email = BCrypt.Net.BCrypt.HashPassword("blank_admin"),
                Username = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("332skd@sdfjIJSKJC")
            });
            
            entity.HasData(new User
            {
                Id = 2,
                Name = "System",
                Surname = "Account",
                Email = BCrypt.Net.BCrypt.HashPassword("blank"),
                Username = "deleted_user",
                Password = BCrypt.Net.BCrypt.HashPassword("WK21QQQQSSS!!@A")
            });
            
        });

        // Customer Entity Configuration (inherits from User)
        modelBuilder.Entity<Customer>(entity =>
        {
            // Configure table mapping and primary key (which is also a foreign key to User).
            entity.ToTable("Customer");
            entity.HasKey(x => x.UserId);

            // Configure Customer-specific property constraint.
            entity.Property(x => x.DOB).IsRequired();

            // Configure the Address as an owned entity for Customer.
            entity.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(100).IsRequired();
                address.Property(a => a.City).HasMaxLength(50).IsRequired();
                address.Property(a => a.HouseNumber).IsRequired();
                address.Property(a => a.PostalCode).HasMaxLength(20).IsRequired();
                address.Property(a => a.Country).HasMaxLength(50).IsRequired();
            });

            // Define the one-to-one relationship with the base User entity, with cascade delete.
            entity.HasOne(x => x.User)
                .WithOne(x => x.Customer)
                .HasForeignKey<Customer>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasData(new Customer
            {
                UserId = 2,
                DOB = DateTime.Now
            });

        });

        // Genre Entity Configuration
        modelBuilder.Entity<Genre>(entity =>
        {
            // Configure table mapping, primary key, and auto-incrementing ID.
            entity.ToTable("Genre");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            // Configure property lengths and nullability.
            entity.Property(x => x.Name).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500).IsRequired();

            // Add SQL CHECK constraints.
            entity.ToTable(t => t.HasCheckConstraint("CK_Genre_Name", "[Name] <> ''"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Genre_Description", "[Description] <> ''"));
            
            entity.HasData(
                new Genre { Id = 1, Name = "Science Fiction", Description = "Explores speculative concepts such as futuristic science and technology, space travel, time travel, and extraterrestrial life, often drawing on social and philosophical themes (e.g., 'Wool' by Hugh Howey)." },
                new Genre { Id = 2, Name = "Dystopian", Description = "A subgenre of speculative fiction that explores social and political structures where the norm is an unjust and oppressive society, often post-apocalyptic or totalitarian (highly relevant to 'Wool' by Hugh Howey)." },
                new Genre { Id = 3, Name = "Post-Apocalyptic", Description = "Set in a world or civilization after a catastrophic event that has drastically altered the climate, landscape, or society, often focusing on survival and rebuilding (a core element of 'Wool' by Hugh Howey)." },
                new Genre { Id = 4, Name = "Spiritual Fiction", Description = "Focuses on themes of spiritual growth, enlightenment, and the search for meaning, often incorporating elements of different belief systems or personal journeys (e.g., 'Mutant Message Down Under' by Marlo Morgan)." },
                new Genre { Id = 5, Name = "New Age", Description = "A broad category often dealing with spirituality, self-help, holistic health, and alternative beliefs, sometimes incorporating fictional narratives to convey these concepts (relevant to 'Mutant Message Down Under')." },
                new Genre { Id = 6, Name = "Literary Fiction", Description = "Emphasizes character development, intricate plots, and often explores deep human experiences and societal issues with a focus on prose style and artistic merit (e.g., 'Left Neglected' by Lisa Genova)." },
                new Genre { Id = 7, Name = "Contemporary Fiction", Description = "Set in the modern era and deals with realistic characters, settings, and problems that reflect current societal issues, personal struggles, and relationships (applicable to 'Left Neglected')." },
                new Genre { Id = 8, Name = "Medical Drama", Description = "A genre often found in fiction that focuses on medical professionals, patients, and the ethical or personal dilemmas faced in healthcare settings (a significant theme in 'Left Neglected')." },
                new Genre { Id = 9, Name = "Fantasy", Description = "Features magical elements, mythical creatures, and often takes place in fictional worlds with unique rules and societies. Subgenres include high fantasy, urban fantasy, and dark fantasy." },
                new Genre { Id = 10, Name = "Thriller", Description = "Designed to evoke excitement, suspense, and a high level of anticipation. Often involves crime, espionage, or psychological tension with a race against time." },
                new Genre { Id = 11, Name = "Historical Fiction", Description = "Set in the past and often incorporates factual historical events, figures, or settings into a fictional narrative, aiming for accuracy in depicting the time period." },
                new Genre { Id = 12, Name = "Romance", Description = "Primarily focuses on the romantic relationship between two or more characters, often with an emotionally satisfying and optimistic ending. Subgenres include contemporary, historical, and paranormal." }
            );

            
        });

        // UsedItem Entity Configuration (derived from Item via TPH)
        modelBuilder.Entity<UsedItem>(entity =>
        {
            // Configure properties specific to UsedItem, including enum conversion for Condition.
            entity.Property(x => x.Condition).IsRequired().HasConversion<int>();
            entity.Property(x => x.HasAnnotations).IsRequired();
        });

        // NewItem Entity Configuration (derived from Item via TPH)
        modelBuilder.Entity<NewItem>(entity =>
        {
            // Configure properties specific to NewItem.
            entity.Property(x => x.IsSealed).IsRequired();
        });

        // Employee Entity Configuration (inherits from User)
        modelBuilder.Entity<Employee>(entity =>
        {
            // Configure table mapping and primary key (which is also a foreign key to User).
            entity.ToTable("Employee");
            entity.HasKey(x => x.UserId);

            // Configure property constraints and enum conversion for Experience.
            entity.Property(x => x.Salary).IsRequired();
            entity.Property(x => x.Experience).HasConversion<int>().IsRequired();

            // Add SQL CHECK constraints for Salary and Experience range.
            entity.ToTable(t => t.HasCheckConstraint("CK_Employee_Salary", "[Salary] > 0"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Employee_Experience", "[Experience] BETWEEN 0 AND 2"));

            // Define the one-to-one relationship with the base User entity, with cascade delete.
            entity.HasOne(x => x.User)
                .WithOne(x => x.Employee)
                .HasForeignKey<Employee>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasData(new Employee
            {
                UserId = 1,
                Experience = Experience.Senior,
                Salary = 5000.0
            });
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshToken");
            entity.HasKey(x => x.UserId);
            
            entity.Property(x => x.Token).IsRequired();
            entity.Property(x => x.Expiration).IsRequired();

            entity.HasIndex(x => x.Token)
                .IsUnique();
            
            entity.HasOne(x => x.User)
                .WithOne(x => x.RefreshToken)
                .HasForeignKey<RefreshToken>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

    }
}