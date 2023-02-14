using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderAPI.Models;
using System.Data.Common;

namespace Order.FunctionalTests.TestClient
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<OrderDbContext>));

                services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbConnection));

                services.Remove(dbConnectionDescriptor);

                services.AddEntityFrameworkInMemoryDatabase();

                // Create a new service provider.
                var provider = services
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<OrderDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDB");
                    options.UseInternalServiceProvider(provider);
                });

                // Build the service provider.
                var serviceProvider = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                using var scope = serviceProvider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var orderDbContext = scopedServices.GetRequiredService<OrderDbContext>();

                // Ensure the database is created.
                orderDbContext.Database.EnsureCreated();
            });

            builder.UseEnvironment("Development");
        }
    }
}
