using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace ProductApi.Test
{
    public class ProductWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>  where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => {
                var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ProductDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ProductDbContext>(options =>
                {
                    options.UseSqlServer(GetConnectionString());
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ProductDbContext>();

                try
                {
                    // Apply pending migrations
                    db.Database.Migrate();

                    // Optionally, seed the database with test data
                    SeedData(db);
                }
                catch (Exception ex)
                {
                    // Log errors during migration
                    var logger = scopedServices.GetRequiredService<ILogger<ProductWebApplicationFactory<TStartup>>>();
                    logger.LogError(ex, "An error occurred applying migrations or seeding the database.");
                }
            });
        }

        private string GetConnectionString()
        {
            // Build the configuration to access the connection string
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration.GetConnectionString("ProductDbContext")!;
        }

        private void SeedData(ProductDbContext db)
        {
            // Example of seeding data into the database
            if (!db.Product.Any())
            {
                db.Product.AddRange(
                    new Product { Name = "Product 1", Quantity = 1 },
                    new Product { Name = "Product 2", Quantity = 1 },
                    new Product { Name = "Product 3", Quantity = 1 }
                );

                db.SaveChanges();
            }
        }
    }
}
